using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class Company : Agent
    {
        public bool is_player { get; private set; }
        public CompanyColor color { get; private set; }

        public Company(bool is_player, CompanyColor color, FIS fis) : base(fis)
        {
            this.is_player = is_player;
            this.color = color;
            SetName("Company");
        }

        [JsonConstructor]
        public Company(bool is_player, CompanyColor color, string name, AgentResources resources, AgentState state, Reference<FIS> ref_fis, int id) : base(name, resources, state, ref_fis, id)
        {
            this.is_player = is_player;
            this.color = color;
        }

        public override void Setup()
        {
            base.Setup();

            GetBank().OpenAccount(this);
            Receive(GetBank().PrintMoney(MEnv.init_company_money));

            AddNoneFactory();
        }

        public override void Destroy()
        {
            
        }

        protected override AgentResources GetAgentResources()
        {
            return new CommonResources(this);
        }

        public override double Pay(double amount)
        {
            base.Pay(amount);
            if(is_player)
            {
                GetNotifications().Notify(NotificationType.ChangePlayerMoney, GetM1());
            }
            return amount;
        }

        public override double Receive(double amount)
        {
            base.Receive(amount);
            if (is_player)
            {
                GetNotifications().Notify(NotificationType.ChangePlayerMoney, GetM1());
            }
            return amount;
        }

        public virtual void Act()
        {
            Land land = GetCheapestLand();
            Food food = GetMaxFood();
            
            if(land != null)
            {
                double cost = 0;
                if(land.GetFactory() != null)
                {
                    if(land.GetFactory().GetOwner() == this)
                    {
                        cost += ((FoodFactory)land.GetFactory()).GetFoodFactoryCoreWithMarketPrices().GetDailyNetIncome() * MEnv.loan_base_term;
                    }
                    else
                    {
                        cost += GetFIS().GetDestructCost((FoodFactory)land.GetFactory());
                    }
                }

                cost += GetFIS().GetConstructCost(food, GetFactoryCounter().GetSize(), this);

                double profit = GetFIS().CreateFoodFactoryCore(food, this).GetDailyNetIncome() * MEnv.loan_base_term - cost;
                if(profit > 0 && GetM1() >= GetFIS().GetTotalConstructCost(food, land.pos, this))
                {
                    GetFIS().Plan(land.pos, food, this);
                }
            }
        }

        public Land GetCheapestLand()
        {
            List<Land> lands = GetResourcesCaches().GetPossibleConstructLands();
            if(lands.Count > 0)
            {
                if(lands.Any(x => x.GetFactory() == null))
                {
                    List<Land> no_factory_lands = lands.Where(x => x.GetFactory() == null).ToList();
                    return no_factory_lands[GetRand().Next(no_factory_lands.Count)];
                }
                else
                {
                    Dictionary<Land, double> costs = new Dictionary<Land, double>();

                    List<Land> my_factory_lands = lands.Where(x => x.GetFactory().GetOwner() == this).ToList();
                    my_factory_lands.ForEach(x => costs.Add(x, ((FoodFactory)x.GetFactory()).GetFoodFactoryCoreWithMarketPrices().GetDailyNetIncome() * MEnv.loan_base_term));

                    List<Land> other_factory = lands.Where(x => x.GetFactory().GetOwner() != this).ToList();
                    other_factory.ForEach(x => costs.Add(x, GetFIS().GetDestructCost((FoodFactory)x.GetFactory())));

                    double min_cost = costs.Min(x => x.Value);
                    List<Land> min_lands = costs.Where(x => x.Value == min_cost).Select(x => x.Key).ToList();
                    return min_lands[GetRand().Next(min_lands.Count)];
                }
            }
            else
            {
                return null;
            }
        }

        Food GetMaxFood()
        {
            Dictionary<Food, double> profits = new Dictionary<Food, double>();

            List<Food> foods = GetResourcesCaches().GetAllFood();
            foreach(Food food in foods)
            {
                if (food.output_product_id < Enum.GetValues(typeof(ProductType)).Length)
                {
                    profits.Add(food, GetFIS().CreateFoodFactoryCore(food, this).GetDailyNetIncome() * MEnv.loan_base_term - GetFIS().GetConstructCost(food, GetFactoryCounter().GetSize(), this));
                }
                else
                {
                    double company_num = GetCompanies().GetAllCompanies().Count;
                    double average_days = 1.0 * MEnv.max_food_count / (MEnv.food_probability * (company_num / MEnv.cpu_food_research_rate));

                    profits.Add(food, GetFIS().CreateFoodFactoryCore(food, this).GetDailyNetIncome() * MEnv.research_food_profit_rate * Math.Min(average_days, MEnv.loan_base_term) - GetFIS().GetConstructCost(food, GetFactoryCounter().GetSize(), this));
                }
            }

            double max_profit = profits.Max(x => x.Value);
            return profits.First(x => x.Value == max_profit).Key;
        }
    }
}
