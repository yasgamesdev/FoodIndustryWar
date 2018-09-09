using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;

public class TopBuildButtonScript : UIScript {
    [SerializeField]
    Slider sales_slider, profit_slider;
    [SerializeField]
    Text top_text, bot_text;
    [SerializeField]
    GameObject balloon;
    [SerializeField]
    Text balloon_text;

    Food food;
    BuilderScript builder_script;

    public void Init(Food food)
    {
        base.Start();

        this.food = food;
        builder_script = GameObject.Find("Builder").GetComponent<BuilderScript>();

        FoodFactoryCore core = fis.CreateFoodFactoryCore(food, fis.GetPlayerCompany());
        double margin = core.GetNetProfitMargin();

        sales_slider.value = (1.0f - (float)margin);
        profit_slider.value = (float)margin;

        top_text.text = food.food_name;
        bot_text.text = $"{core.GetDailyNetIncome():#,0}";

        GetComponent<Button>().onClick.AsObservable().Subscribe(x =>
        {
            builder_script.SetFood(food);
        });

        string text = "";
        foreach(InputRate input in food.inputs.GetInputRates())
        {
            text += $"{fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetFoodName(input.product_id)}{input.rate:F1}\n";
        }
        if(text == "")
        {
            text = "なし";
        }
        balloon_text.text = text;

        balloon.SetActive(false);
    }

    public void PointEnter()
    {
        balloon.SetActive(true);
    }

    public void PointExit()
    {
        balloon.SetActive(false);
    }
}
