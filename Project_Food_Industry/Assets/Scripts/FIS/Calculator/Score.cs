using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Score : ID
    {
        Prices prices;

        public int product_id { get; private set; }
        public double demand_amount { get; private set; }
        public double supply_amount { get; private set; }
        public double optimized_demand_amount { get; private set; }
        public double optimized_supply_amount { get; private set; }
        public double optimized_price { get; private set; }
        public double score { get; private set; }

        public List<IDemandFunc> demands { get; private set; }
        public List<ISupplyFunc> supplies { get; private set; }

        List<int> affected = new List<int>();

        public Score(Prices prices, int product_id, FIS fis) : base(fis)
        {
            this.prices = prices;
            this.product_id = product_id;

            //demand
            demands = new List<IDemandFunc>();
            GetResourcesCaches().GetDemandFactories(product_id).ForEach(x => demands.Add(x.GetIDemandFunc(product_id, prices)));

            //supply
            supplies = new List<ISupplyFunc>();
            GetResourcesCaches().GetSupplyFactories(product_id).ForEach(x => supplies.Add(x.GetISupplyFunc(prices)));

            //affected
            if (product_id == (int)ProductType.Labor)
            {
                affected.AddRange(GetResourcesCaches().GetMarketProductID());
            }
            else
            {
                //self
                affected.Add(product_id);

                //output
                foreach (Factory factory in GetResourcesCaches().GetDemandFactories(product_id))
                {
                    affected.Add(factory.GetSupplyProductID());
                }

                //labor
                affected.Add((int)ProductType.Labor);
            }
            affected = affected.Distinct().ToList();
        }

        public List<int> GetAffected()
        {
            return affected;
        }

        public async Task Calculate()
        {
            double price = prices.GetPrice(product_id);
            optimized_price = price;

            demand_amount = demands.Sum(x => x.GetDemandAmount(product_id, price));
            supply_amount = supplies.Sum(x => x.GetSupplyAmount(product_id, price));

            if ((demand_amount == supply_amount) || (price == 0 && supply_amount > demand_amount))
            {
                optimized_demand_amount = demand_amount;
                optimized_supply_amount = supply_amount;
            }
            else
            {
                double distance = 0;
                bool down = demand_amount < supply_amount ? true : false;

                if (supply_amount > demand_amount)
                {
                    distance = price * 0.5;
                }
                else
                {
                    distance = MEnv.init_distance;
                }

                while (distance >= MEnv.optimize_price_accuracy)
                {
                    optimized_price += (down ? -distance : distance);
                    if (optimized_price < 0)
                    {
                        optimized_price = 0;
                    }

                    optimized_demand_amount = demands.Sum(x => x.GetDemandAmount(product_id, optimized_price));
                    optimized_supply_amount = supplies.Sum(x => x.GetSupplyAmount(product_id, optimized_price));

                    if (optimized_demand_amount == optimized_supply_amount)
                    {
                        break;
                    }
                    else
                    {
                        if (optimized_price == 0)
                        {
                            if (optimized_supply_amount > optimized_demand_amount)
                            {
                                break;
                            }
                            else
                            {
                                down = false;
                                distance *= 0.5;
                            }
                        }
                        else
                        {
                            if ((optimized_demand_amount < optimized_supply_amount && !down) || (optimized_demand_amount > optimized_supply_amount && down))
                            {
                                down = !down;
                                distance *= 0.5;
                            }
                        }
                    }
                }
            }

            SetScore();
        }

        void SetScore()
        {
            double price_diff = Math.Abs(prices.GetPrice(product_id) - optimized_price);
            double amount_diff = Math.Abs(demand_amount - supply_amount);

            double price_score = price_diff / MEnv.price_diff_limit;
            double amount_score = amount_diff / MEnv.amount_diff_limit;

            if (double.IsPositiveInfinity(amount_diff))
            {
                score = price_score * 4.0;
            }
            else
            {
                score = (price_score + amount_score) * 0.5;
            }
        }

        public void SetOptimizedPrice()
        {
            //Lib.Debug($" {(ProductType)product_id}, { prices.GetPrice(product_id)} > {optimized_price}");

            prices.SetPrice(product_id, optimized_price);
        }
    }
}
