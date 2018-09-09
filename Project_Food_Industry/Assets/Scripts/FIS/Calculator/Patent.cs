using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Patent
    {
        Dictionary<Agent, double> patent_sum = new Dictionary<Agent, double>();

        public Patent(FoodFactory factory)
        {
            Agent owner = factory.GetOwner();
            Bank bank = (Bank)factory.GetFIS().GetModule(ModuleType.Bank);

            if(factory.GetFood().GetOwner() != owner && factory.GetFood().GetOwner() != bank)
            {
                patent_sum.Add(factory.GetFood().GetOwner(), MEnv.patent_rate);
            }

            foreach(Technology technology in factory.GetTechnologies().Where(x => x.GetOwner() != owner && x.GetOwner() != bank))
            {
                if(!patent_sum.ContainsKey(technology.GetOwner()))
                {
                    patent_sum.Add(technology.GetOwner(), 0.0);
                }

                patent_sum[technology.GetOwner()] += MEnv.patent_rate;
            }
        }

        public Patent(Agent owner, Bank bank, Food food, List<Technology> technologies)
        {
            if (food.GetOwner() != owner && food.GetOwner() != bank)
            {
                patent_sum.Add(food.GetOwner(), MEnv.patent_rate);
            }

            foreach (Technology technology in technologies.Where(x => x.GetOwner() != owner && x.GetOwner() != bank))
            {
                if (!patent_sum.ContainsKey(technology.GetOwner()))
                {
                    patent_sum.Add(technology.GetOwner(), 0.0);
                }

                patent_sum[technology.GetOwner()] += MEnv.patent_rate;
            }
        }

        public double ReceiveWithPatent(Agent owner, double amount)
        {
            owner.Receive(amount * (1.0 - GetTotalPatentRate()));
            patent_sum.ToList().ForEach(x => x.Key.Receive(amount * x.Value));

            return amount;
        }

        public double GetTotalPatentRate()
        {
            return patent_sum.Values.Sum();
        }
    }
}
