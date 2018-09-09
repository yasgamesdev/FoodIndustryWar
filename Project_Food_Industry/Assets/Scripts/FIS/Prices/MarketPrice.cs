using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;

namespace Food_Industry
{
    public class MarketPrice : ID
    {
        [JsonProperty] Prices prices;

        public MarketPrice(FIS fis) : base(fis)
        {
            prices = new Prices();
        }

        [JsonConstructor]
        public MarketPrice(Prices prices, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.prices = prices;
        }

        public override void AfterConstructor()
        {
            GetNotifications().GetSubject(NotificationType.AddFood).Subscribe<Parameters>(x => {
                Food food = x.Get<Food>(0);
                prices.AddPrice(food.output_product_id);
            });

            GetNotifications().GetSubject(NotificationType.RemoveFood).Subscribe<Parameters>(x => {
                Food food = x.Get<Food>(0);
                prices.RemovePrice(food.output_product_id);
            });
        }

        public Prices GetDeepCopy()
        {
            return prices.GetDeepCopy();
        }

        public void Merge(Prices merge_prices)
        {
            prices.Merge(merge_prices);
        }

        public Prices GetPrices()
        {
            return prices;
        }

        public double GetPrice(int product_id)
        {
            return prices.GetPrice(product_id);
        }
    }
}
