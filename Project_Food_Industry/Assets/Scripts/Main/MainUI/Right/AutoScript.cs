using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class AutoScript : UIScript
{
    [SerializeField]
    GameObject next_button;
    [SerializeField]
    Toggle auto_toggle;

    float timer;

    protected override void Start()
    {
        base.Start();

        fis.GetSubject(NotificationType.GameOver).Subscribe(x => {
            SetInteractable(false);
        });

        auto_toggle.OnValueChangedAsObservable().Subscribe<bool>(x => {
            if (x)
            {
                timer = 0.0f;
            }
            else
            {
                next_button.GetComponent<RectTransform>().localRotation = Quaternion.identity;
            }
        });
    }

    public void SetInteractable(bool value)
    {
        if(!value)
        {
            auto_toggle.isOn = false;
        }
        auto_toggle.interactable = value;
    }

    void Update()
    {
        if (auto_toggle.isOn)
        {
            if (timer == 0.0f)
            {
                timer += 0.0001f;
            }
            else
            {
                timer = timer + Time.deltaTime > 1.0f ? 1.0f : timer + Time.deltaTime;
                next_button.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, timer * -360.0f);
            }

            if (timer >= 1.0f)
            {
                timer = 0.0f;
                next_button.GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}
