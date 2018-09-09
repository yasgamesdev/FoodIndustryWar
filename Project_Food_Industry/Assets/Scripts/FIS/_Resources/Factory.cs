using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public abstract class Factory : Resource
    {
        [JsonProperty] Reference<Land> ref_land;
        public DateTime construct_date { get; private set; }
        public double construct_cost { get; private set; }

        public Factory(Land land, double construct_cost, Agent owner) : base(owner)
        {
            if(land != null)
            {
                ref_land = new Reference<Land>(land);
            }
            construct_date = GetDate().GetCurrentDateTime();
            this.construct_cost = construct_cost;
        }

        [JsonConstructor]
        public Factory(Reference<Land> ref_land, DateTime construct_date, double construct_cost, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.ref_land = ref_land;
            this.construct_date = construct_date;
            this.construct_cost = construct_cost;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            if(ref_land != null)
            {
                ref_land.SetReference(builder);
            }
        }

        public Land GetLand()
        {
            if(ref_land == null)
            {
                return null;
            }
            else
            {
                return ref_land.Get();
            }
        }

        public abstract int GetSupplyProductID();
        public abstract List<int> GetDemandProductID();
        public abstract IDemandFunc GetIDemandFunc(int product_id, Prices prices);
        public abstract ISupplyFunc GetISupplyFunc(Prices prices);

        public override double GetAssets()
        {
            return construct_cost;
        }

        //public override void Destroy()
        //{
        //    if(GetLand() != null)
        //    {
        //        GetLand().ClearFactory();
        //    }
        //}
    }
}
