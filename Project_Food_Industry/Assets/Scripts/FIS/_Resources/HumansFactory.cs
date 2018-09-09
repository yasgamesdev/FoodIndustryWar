using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class HumansFactory : Factory
    {
        public HumansFactory(Agent owner) : base(null, 0, owner)
        {

        }

        [JsonConstructor]
        public HumansFactory(Reference<Land> ref_land, DateTime construct_date, double construct_cost, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_land, construct_date, construct_cost, ref_owner, ref_fis, id)
        {

        }

        public override List<int> GetDemandProductID()
        {
            return GetResourcesCaches().GetConsumerProductID();
        }

        public override int GetSupplyProductID()
        {
            return (int)ProductType.Labor;
        }

        public override IDemandFunc GetIDemandFunc(int product_id, Prices prices)
        {
            return new HumansDemandCore(product_id, GetOwner());
        }

        public override ISupplyFunc GetISupplyFunc(Prices prices)
        {
            return new HumansSupplyCore(GetOwner());
        }
    }
}
