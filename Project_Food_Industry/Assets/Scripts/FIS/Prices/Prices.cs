using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Prices
    {
        [JsonProperty] Dictionary<int, double> prices;

        public Prices()
        {
            prices = new Dictionary<int, double>();
        }

        [JsonConstructor]
        public Prices(Dictionary<int, double> prices)
        {
            this.prices = prices;
        }

        public double GetPrice(int product_id, int estimate_product_id, double estimate_price)
        {
            if (product_id == estimate_product_id)
            {
                return estimate_price;
            }
            else
            {
                return prices[product_id];
            }
        }

        public double GetPrice(int product_id)
        {
            return prices[product_id];
        }

        public void SetPrice(int product_id, double price)
        {
            prices[product_id] = price;
        }

        public Prices GetDeepCopy()
        {
            return new Prices(new Dictionary<int, double>(prices));
        }

        public void Merge(Prices merge_prices)
        {
            prices.Keys.ToList().ForEach(x => prices[x] = merge_prices.GetPrice(x));
        }

        public void AddPrice(int product_id)
        {
            prices.Add(product_id, 1.0);
        }

        public void RemovePrice(int product_id)
        {
            prices.Remove(product_id);
        }
    }
}
