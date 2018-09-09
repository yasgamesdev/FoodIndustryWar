using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class BubbleScript : UIScript
{
    [SerializeField]
    Toggle bubble_toggle, sound_toggle;
    [SerializeField]
    GameObject bubble_prefab;
    [SerializeField]
    Transform bubble_parent;

    protected override void Start()
    {
        base.Start();

        fis.GetSubject(NotificationType.EndNext).Subscribe(x => {
            if (bubble_toggle.isOn)
            {
                SetBubbles();
            }
        });
    }

    void SetBubbles()
    {
        List<FoodFactory> factories = fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetConstructedFoodFactory();
        double average = factories.Count > 0 ? factories.Average(x => x.GetFoodFactoryCoreWithMarketPrices().GetDailyNetIncome()) : 0.0;

        int count = 0;
        foreach(FoodFactory factory in factories.Where(x => x.GetOwner() == fis.GetPlayerCompany()))
        {
            GameObject instance = Instantiate(bubble_prefab, bubble_parent);
            instance.GetComponent<BubblePrefabScript>().Init(factory, average, sound_toggle.isOn && count < 6);
            count++;
        }
    }
}
