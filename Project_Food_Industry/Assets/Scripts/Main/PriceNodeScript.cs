using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PriceNodeScript : MonoBehaviour
{
    [SerializeField]
    Text name_text, demand_text, supply_text, price_text;

    PriceUIScript parent;
    public int product_id { get; private set; }
    public string food_name { get; private set; }

    FIS fis;

    public void Init(PriceUIScript parent, int product_id, string food_name)
    {
        this.product_id = product_id;
        this.food_name = food_name;

        name_text.text = food_name;

        fis = FISLoader.GetFIS();

        GetComponent<Button>().onClick.AsObservable().Subscribe(x => {
            parent.Select(this);
        });
    }

    public void Reload()
    {
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

            //double labor_num = supplies.Sum(x => ((FoodFactoryCore)x).GetLaborNum(product_id, price));
            //double net_income = supplies.Sum(x => ((FoodFactoryCore)x).GetDailyNetIncome());
            //double margin = supplies.Average(x => ((FoodFactoryCore)x).GetNetProfitMargin());

            demand_text.text = $"{demand_amount:F0}";
            supply_text.text = $"{supply_amount:F0}";
            price_text.text = $"{price:F0}";
            //Lib.Debug($"{(ProductType)product_id}: demand:{demand_amount:F0} -> supply:{supply_amount:F0} labor:{labor_num:F0} price:{price:F2}, netincome:{net_income:F2}, margin:{margin:F2}");
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

            demand_text.text = $"{demand_amount:F0}";
            supply_text.text = $"{supply_amount:F0}";
            price_text.text = $"{price:F0}";
            //Lib.Debug($"{(ProductType)product_id}: demand:{demand_amount:F0} -> supply:{supply_amount:F0} price:{price:F2}");
        }
    }
}
