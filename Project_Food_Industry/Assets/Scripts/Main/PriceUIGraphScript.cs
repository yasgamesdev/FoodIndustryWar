using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PriceUIGraphScript : MonoBehaviour
{
    [SerializeField]
    Text amount_text, price_text, bot_price_text, bot_demand_text, bot_supply_text, bot_netincome_text, bot_margin_text;
    [SerializeField]
    GameObject color_dot_prefab;
    [SerializeField]
    Transform canvas;

    List<GameObject> dots = new List<GameObject>();

    FIS fis;

    public void Reload(int product_id)
    {
        fis = FISLoader.GetFIS();
        Clear();

        if (product_id != (int)ProductType.Labor)
        {
            List<IDemandFunc> demands = new List<IDemandFunc>();
            List<ISupplyFunc> supplies = new List<ISupplyFunc>();
            MarketPrice market_price = fis.GetModule<MarketPrice>(ModuleType.MarketPrice);
            double price = market_price.GetPrice(product_id);

            fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetDemandFactories(product_id).ForEach(x => demands.Add(x.GetIDemandFunc(product_id, market_price.GetPrices())));
            double demand_amount = demands.Sum(x => x.GetDemandAmount(product_id, price));

            fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetSupplyFactories(product_id).ForEach(x => supplies.Add(x.GetISupplyFunc(market_price.GetPrices())));
            double supply_amount = supplies.Sum(x => x.GetSupplyAmount(product_id, price));

            double labor_num = supplies.Sum(x => ((FoodFactoryCore)x).GetLaborNum(product_id, price));
            double net_income = supplies.Sum(x => ((FoodFactoryCore)x).GetDailyNetIncome());
            double margin = supplies.Average(x => ((FoodFactoryCore)x).GetNetProfitMargin());

            DrawCanvas(demands, supplies, price, supply_amount, product_id);

            bot_price_text.text = $"{price:#,0}";
            bot_demand_text.text = $"{demand_amount:F0}";
            bot_supply_text.text = $"{supply_amount:F0}";
            bot_netincome_text.text = $"{net_income:#,0}";
            bot_margin_text.text = $"{margin:P}";
        }
        else
        {
            List<IDemandFunc> demands = new List<IDemandFunc>();
            List<ISupplyFunc> supplies = new List<ISupplyFunc>();
            MarketPrice market_price = fis.GetModule<MarketPrice>(ModuleType.MarketPrice);
            double price = market_price.GetPrice(product_id);

            fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetDemandFactories(product_id).ForEach(x => demands.Add(x.GetIDemandFunc(product_id, market_price.GetPrices())));
            double demand_amount = demands.Sum(x => x.GetDemandAmount(product_id, price));

            fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetSupplyFactories(product_id).ForEach(x => supplies.Add(x.GetISupplyFunc(market_price.GetPrices())));
            double supply_amount = supplies.Sum(x => x.GetSupplyAmount(product_id, price));

            DrawCanvas(demands, supplies, price, supply_amount, product_id);

            bot_price_text.text = $"{price:#,0}";
            bot_demand_text.text = $"{demand_amount:F0}";
            bot_supply_text.text = $"{supply_amount:F0}";

            bot_netincome_text.text = $"-";
            bot_margin_text.text = $"-";
        }
    }

    void Clear()
    {
        dots.ForEach(x => Destroy(x));
        dots.Clear();
    }

    void DrawCanvas(List<IDemandFunc> demands, List<ISupplyFunc> supplies, double price, double amount, int product_id)
    {
        amount_text.text = $"{amount:F0}";
        price_text.text = $"{price:#,0}";

        if(amount == 0)
        {
            return;
        }

        int dot_num = 50;
        float canvas_size = 150.0f;

        for(int i=0; i< dot_num; i++)
        {
            if(i != dot_num - 1)
            {
                double demand_amount_A = demands.Sum(x => x.GetDemandAmount(product_id, (price * 2.0) * (1.0 / dot_num) * i));
                double demand_amount_B = demands.Sum(x => x.GetDemandAmount(product_id, (price * 2.0) * (1.0 / dot_num) * (i + 1)));

                if(double.IsInfinity(demand_amount_A) || double.IsInfinity(demand_amount_B))
                {
                    continue;
                }
                else
                {
                    DrawLine(new Vector3(canvas_size * (1.0f / dot_num) * i, (float)(demand_amount_A / amount) * (canvas_size * 0.5f), 0), new Vector3(canvas_size * (1.0f / dot_num) * (i + 1), (float)(demand_amount_B / amount) * (canvas_size * 0.5f), 0), new Color(92.0f / 255.0f, 1.0f, 0));
                }
            }
        }

        for (int i = 0; i < dot_num; i++)
        {
            if (i != dot_num - 1)
            {
                double supply_amount_A = supplies.Sum(x => x.GetSupplyAmount(product_id, (price * 2.0) * (1.0 / dot_num) * i));
                double supply_amount_B = supplies.Sum(x => x.GetSupplyAmount(product_id, (price * 2.0) * (1.0 / dot_num) * (i + 1)));

                if (double.IsInfinity(supply_amount_A) || double.IsInfinity(supply_amount_B))
                {
                    continue;
                }
                else
                {
                    DrawLine(new Vector3(canvas_size * (1.0f / dot_num) * i, (float)(supply_amount_A / amount) * (canvas_size * 0.5f), 0), new Vector3(canvas_size * (1.0f / dot_num) * (i + 1), (float)(supply_amount_B / amount) * (canvas_size * 0.5f), 0), new Color(192.0f / 255.0f, 0.0f, 1.0f));
                }
            }
        }
    }

    void DrawLine(Vector3 pointA, Vector3 pointB, Color color)
    {
        Vector3 differenceVector = pointB - pointA;

        GameObject dot_instance = Instantiate(color_dot_prefab, canvas);
        dot_instance.GetComponent<Image>().color = color;
        dot_instance.GetComponent<RectTransform>().sizeDelta = new Vector2(differenceVector.magnitude, 4);
        dot_instance.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
        dot_instance.GetComponent<RectTransform>().localPosition = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        dot_instance.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, angle);
        dots.Add(dot_instance);
    }
}
