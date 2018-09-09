using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class Cash : Resource
    {
        public double amount { get; private set; }

        public Cash(Agent owner) : base(owner)
        {
            amount = 0;
        }

        [JsonConstructor]
        public Cash(double amount, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.amount = amount;
        }

        public double Pay(double amount)
        {
            this.amount -= amount;
            if (this.amount < 0)
            {
                this.amount = 0;
            }

            return amount;
        }

        public double Receive(double amount)
        {
            this.amount += amount;

            return amount;
        }

        public void SetAmount(double amount)
        {
            this.amount = amount;
        }

        public override double GetAssets()
        {
            return amount;
        }

        public override double GetFinalAssetValue()
        {
            return amount;
        }
    }
}
