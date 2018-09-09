using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class CommonResources : AgentResources
    {
        public CommonResources(Agent owner) : base(owner)
        {

        }

        [JsonConstructor]
        public CommonResources(Reference<Agent> ref_owner, Dictionary<ResourceType, List<Resource>> resource_dict, Reference<FIS> ref_fis, int id) : base(ref_owner, resource_dict, ref_fis, id)
        {

        }

        public override double Pay(double amount)
        {
            if (amount > GetM1())
            {
                double shortage = amount - GetM1();
                Receive(GetBank().PrintMoney(shortage));

                GetOwner().SetState(AgentState.Bankrupt);
            }

            return GetPassbook().Withdraw(amount);
        }

        public override double Receive(double amount)
        {
            return GetPassbook().Deposit(amount);
        }

        Passbook GetPassbook()
        {
            return (Passbook)GetResources(ResourceType.Passbook)[0];
        }
    }
}
