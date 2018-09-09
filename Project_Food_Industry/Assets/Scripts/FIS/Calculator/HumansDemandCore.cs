using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class HumansDemandCore : IDemandFunc, IGetOwner
    {
        Agent owner;
        double budget;

        public HumansDemandCore(int product_id, Agent owner)
        {
            this.owner = owner;

            ResourcesCaches caches = (ResourcesCaches)owner.GetFIS().GetModule(ModuleType.ResourcesCaches);

            Humans humans = (Humans)owner;
            budget = humans.GetM1() * MEnv.humans_consume_rate / caches.GetConsumerProductID().Count;
        }

        public Agent GetOwner()
        {
            return owner;
        }

        public double GetDemandAmount(int product_id, double price)
        {
            if (price == 0)
            {
                if (budget == 0)
                {
                    return 0;
                }
                else
                {
                    return double.PositiveInfinity;
                }
            }
            else
            {
                return budget / price;
            }
        }

        public double ReceiveWithPatent(double amount)
        {
            return owner.Receive(amount);
        }
    }
}
