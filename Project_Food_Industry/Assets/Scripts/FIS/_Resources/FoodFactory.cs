using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class FoodFactory : Factory
    {
        [JsonProperty] Reference<Food> ref_food;
        [JsonProperty] List<Reference<Technology>> ref_tecs;
        public double size { get; private set; }

        public FoodFactory(Food food, double size, Land land, double construct_cost, Agent owner) : base(land, construct_cost, owner)
        {
            ref_food = new Reference<Food>(food);
            ref_tecs = GetResourcesCaches().GetLatestTechnologies(food).ConvertAll(x => new Reference<Technology>(x));
            this.size = size;
        }

        [JsonConstructor]
        public FoodFactory(Reference<Food> ref_food, List<Reference<Technology>> ref_tecs, double size, Reference<Land> ref_land, DateTime construct_date, double construct_cost, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_land, construct_date, construct_cost, ref_owner, ref_fis, id)
        {
            this.ref_food = ref_food;
            this.ref_tecs = ref_tecs;
            this.size = size;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            ref_food.SetReference(builder);
            ref_tecs.ForEach(x => x.SetReference(builder));
        }

        public Food GetFood()
        {
            return ref_food.Get();
        }

        public List<Technology> GetTechnologies()
        {
            return ref_tecs.ConvertAll(x => x.Get());
        }

        public override List<int> GetDemandProductID()
        {
            List<int> demand_product_id = new List<int>(GetFood().inputs.GetAllProductID());
            if(GetFood().output_product_id != (int)ProductType.None)
            {
                demand_product_id.Add((int)ProductType.Labor);
            }
            
            return demand_product_id;
        }

        public override int GetSupplyProductID()
        {
            return GetFood().output_product_id;
        }

        public double GetOutput()
        {
            double output = GetFood().base_output;
            GetTechnologies().ForEach(x => output += x.output_magnification);
            return output;
        }

        public double GetNewBase()
        {
            return Math.Pow(0.5, 1 / (MEnv.half_output_labor_num * size));
        }

        public Patent GetPatent()
        {
            return new Patent(this);
        }

        public FoodFactoryCore GetFoodFactoryCore(Prices prices)
        {
            return new FoodFactoryCore(GetFood().output_product_id, GetOutput(), GetNewBase(), GetFood().inputs, prices, GetPatent(), GetOwner());
        }

        public FoodFactoryCore GetFoodFactoryCoreWithMarketPrices()
        {
            return new FoodFactoryCore(GetFood().output_product_id, GetOutput(), GetNewBase(), GetFood().inputs, GetMarketPrice().GetPrices(), GetPatent(), GetOwner());
        }

        public override IDemandFunc GetIDemandFunc(int product_id, Prices prices)
        {
            return GetFoodFactoryCore(prices);
        }

        public override ISupplyFunc GetISupplyFunc(Prices prices)
        {
            return GetFoodFactoryCore(prices);
        }

        public override double GetFinalAssetValue()
        {
            return GetFoodFactoryCore(GetMarketPrice().GetPrices()).GetDailyNetIncome() * MEnv.loan_base_term;
        }
    }
}
