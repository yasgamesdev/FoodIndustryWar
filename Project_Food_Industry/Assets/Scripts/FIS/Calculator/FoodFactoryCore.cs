using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class FoodFactoryCore : IDemandFunc, ISupplyFunc, IGetOwner
    {
        Agent owner;

        int output_product_id;
        double output;
        double newbase;
        InputRates inputs;
        Prices prices;
        Patent patent;

        public FoodFactoryCore(int output_product_id, double output, double newbase, InputRates inputs, Prices prices, Patent patent, Agent owner)
        {
            this.owner = owner;

            this.output_product_id = output_product_id;
            this.output = output;
            this.newbase = newbase;
            this.inputs = inputs;
            this.prices = prices;
            this.patent = patent;
        }

        public Agent GetOwner()
        {
            return owner;
        }

        public double ReceiveWithPatent(double amount)
        {
            return patent.ReceiveWithPatent(owner, amount);
        }

        public double GetDemandAmount(int product_id, double price)
        {
            double labor_num = GetLaborNum(product_id, price);

            if (product_id == (int)ProductType.Labor)
            {
                return labor_num;
            }
            else
            {
                return GetInputAmount(labor_num, product_id);
            }
        }

        public double GetSupplyAmount(int product_id, double price)
        {
            double labor_num = GetLaborNum(product_id, price);

            return GetTotalOutput(labor_num);
        }

        public double GetLaborNum(int product_id, double price)
        {
            double wage = prices.GetPrice((int)ProductType.Labor, product_id, price);
            double selling_price = prices.GetPrice(output_product_id, product_id, price);
            double input_cost_per_output = GetInputCostPerOutput(prices, product_id, price);

            double labor_num = selling_price > input_cost_per_output ? Math.Log(wage / ((selling_price - input_cost_per_output) * output)) / Math.Log(newbase) : 0;

            if (labor_num <= 0)
            {
                return 0;
            }
            else
            {
                return labor_num;
            }
        }

        double GetInputAmount(double labor_num, int product_id)
        {
            double total_output = GetTotalOutput(labor_num);

            return inputs.GetInputAmount(total_output, product_id);
        }

        public double GetTotalOutput(double labor_num)
        {
            return output * (Math.Pow(newbase, labor_num) - 1) / Math.Log(newbase);
        }

        public double GetInputCostPerOutput(Prices prices, int product_id, double price)
        {
            return inputs.GetCost(prices, product_id, price);
        }

        public double GetSales()
        {
            double price = prices.GetPrice(output_product_id);
            return price * GetSupplyAmount(output_product_id, price);
        }

        public double GetPay(int product_id)
        {
            double price = prices.GetPrice(product_id);
            return price * GetDemandAmount(product_id, price);
        }

        public double GetInputCost()
        {
            return inputs.GetInputRates().Sum(x => GetPay(x.product_id));
        }

        public double GetLaborCost()
        {
            double price = prices.GetPrice((int)ProductType.Labor);
            return price * GetDemandAmount((int)ProductType.Labor, price);
        }

        public double GetTotalPatent()
        {
            return GetDailyOperatingProfit() * patent.GetTotalPatentRate();
        }

        public virtual double GetDailyOperatingProfit()
        {
            return GetSales() - inputs.GetInputRates().Sum(x => GetPay(x.product_id)) - GetLaborCost();
        }

        public double GetDailyNetIncome()
        {
            return GetDailyOperatingProfit() * (1.0 - patent.GetTotalPatentRate());
        }

        public double GetNetProfitMargin()
        {
            double sales = GetSales();
            if (sales == 0)
            {
                return 0;
            }
            else
            {
                return GetDailyNetIncome() / sales;
            }
        }
    }
}
