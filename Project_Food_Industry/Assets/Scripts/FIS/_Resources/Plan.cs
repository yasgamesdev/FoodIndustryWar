using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Plan : Resource
    {
        [JsonProperty] Reference<Food> ref_food;
        [JsonProperty] Reference<Land> ref_land;
        public double size { get; private set; }
        public double construct_cost { get; private set; }

        public Plan(Food food, Land land, double size, double construct_cost, Agent owner) : base(owner)
        {
            ref_food = new Reference<Food>(food);
            ref_land = new Reference<Land>(land);
            this.size = size;
            this.construct_cost = construct_cost;
        }

        [JsonConstructor]
        public Plan(Reference<Food> ref_food, Reference<Land> ref_land, double size, double construct_cost, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.ref_food = ref_food;
            this.ref_land = ref_land;
            this.size = size;
            this.construct_cost = construct_cost;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            ref_food.SetReference(builder);
            ref_land.SetReference(builder);
        }

        public Food GetFood()
        {
            return ref_food.Get();
        }

        public Land GetLand()
        {
            return ref_land.Get();
        }

        public override double GetAssets()
        {
            return construct_cost;
        }
    }
}
