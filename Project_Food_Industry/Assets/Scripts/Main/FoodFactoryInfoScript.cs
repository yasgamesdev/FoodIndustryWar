using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodFactoryInfoScript : MonoBehaviour
{
    [SerializeField]
    RectTransform sales, cost, labor, patent, netincome;
    [SerializeField]
    Text food_name, price_text, sales_text, cost_text, labor_text, patent_text, netincome_text, inpute_rates_text;
    [SerializeField]
    Transform right_nodes;
    [SerializeField]
    Text base_output_text, size_text;
    [SerializeField]
    GameObject technology_prefab, final_output_prefab;
    [SerializeField]
    Text owner_text;
    [SerializeField]
    GameObject education;

    FoodFactory factory;

    public void Init(FoodFactory factory)
    {
        this.factory = factory;

        FoodFactoryCore core = factory.GetFoodFactoryCoreWithMarketPrices();

        double sales_amount = core.GetSales();

        if(sales_amount == 0)
        {

        }
        else
        {
            float cost_height = 200.0f * (float)(core.GetInputCost() / sales_amount);
            cost.sizeDelta = new Vector2(50.0f, cost_height);
            cost.localPosition = new Vector3(0, 0);

            float labor_height = 200.0f * (float)(core.GetLaborCost() / sales_amount);
            labor.sizeDelta = new Vector2(50.0f, labor_height);
            labor.localPosition = new Vector3(0, -cost_height);

            float patent_height = 200.0f * (float)(core.GetTotalPatent() / sales_amount);
            patent.sizeDelta = new Vector2(50.0f, patent_height);
            patent.localPosition = new Vector3(0, -(cost_height + labor_height));

            float netincome_height = 200.0f * (float)(core.GetDailyNetIncome() / sales_amount);
            netincome.sizeDelta = new Vector2(50.0f, netincome_height);
            netincome.localPosition = new Vector3(0, -(cost_height + labor_height + patent_height));
        }

        food_name.text = factory.GetFood().food_name;
        price_text.text = $"{((MarketPrice)factory.GetFIS().GetModule(ModuleType.MarketPrice)).GetPrice(factory.GetFood().output_product_id):#,0}";

        sales_text.text = $"{core.GetSales():#,0}";
        cost_text.text = $"{core.GetInputCost():#,0}";
        labor_text.text = $"{core.GetLaborCost():#,0}";
        patent_text.text = $"{core.GetTotalPatent():#,0}";
        netincome_text.text = $"{core.GetDailyNetIncome():#,0}";

        InputRates inputs = factory.GetFood().inputs;
        if(inputs.GetInputRates().Count == 0)
        {
            inpute_rates_text.text = $"{Lang.Get("main_ingredients")}\n{Lang.Get("main_none")}";
        }
        else
        {
            string build_text = $"{Lang.Get("main_ingredients")}\n";
            List<InputRate> rates = inputs.GetInputRates();
            ResourcesCaches caches = (ResourcesCaches)factory.GetFIS().GetModule(ModuleType.ResourcesCaches);
            for(int i=0; i<rates.Count; i++)
            {
                build_text += $"{caches.GetFoodName(rates[i].product_id)}{rates[i].rate:F1} ";

                if(i % 3 == 2)
                {
                    build_text += "\n";
                }
            }
            inpute_rates_text.text = build_text;
        }

        base_output_text.text = $"{factory.GetFood().base_output:F1}";

        float right_nodes_ypos = 0.0f;
        foreach(Technology technology in factory.GetTechnologies())
        {
            GameObject technology_node = Instantiate(technology_prefab, right_nodes);
            technology_node.GetComponent<RectTransform>().localPosition = new Vector3(0, right_nodes_ypos);
            right_nodes_ypos -= 20.0f;
            technology_node.transform.Find("Caption").GetComponent<Text>().text = $"{Lang.Get("main_dai")}{technology.generation + 1}{Lang.Get("main_generation")}{Lib.GetTechnologyName(technology.tec_type)}";
            technology_node.transform.Find("Value").GetComponent<Text>().text = $"+{technology.output_magnification:F1}";
        }

        GameObject final_output = Instantiate(final_output_prefab, right_nodes);
        final_output.GetComponent<RectTransform>().localPosition = new Vector3(0, right_nodes_ypos);
        final_output.transform.Find("Value").GetComponent<Text>().text = $"{factory.GetOutput():F1}";

        size_text.text = $"{factory.size:F1}";

        if(factory.GetOwner() is Company)
        {
            CompanyColor color = ((Company)factory.GetOwner()).color;
            owner_text.text = $"<color={color.GetColorNameForTag()}>{color.color_name}\n{factory.GetOwner().name}</color>";
        }
        else
        {
            owner_text.text = $"{factory.GetOwner().name}";
        }
        
        if(GameObject.Find("EducationToggle").GetComponent<Toggle>().isOn)
        {
            education.SetActive(true);
            education.GetComponent<FoodFactoryEducationScript>().Init(factory);
        }
        else
        {
            education.SetActive(false);
        }
    }
}
