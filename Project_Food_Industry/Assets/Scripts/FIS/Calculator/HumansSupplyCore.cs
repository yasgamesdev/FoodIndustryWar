using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class HumansSupplyCore : ISupplyFunc, IGetOwner
    {
        Agent owner;
        double size;
        double supply_border;

        public HumansSupplyCore(Agent owner)
        {
            this.owner = owner;

            Humans humans = (Humans)owner;
            size = humans.size;
            supply_border = humans.GetM1() * MEnv.humans_consume_rate * MEnv.supply_border_rate / size;
        }

        public Agent GetOwner()
        {
            return owner;
        }

        public double GetSupplyAmount(int product_id, double price)
        {
            if (price >= supply_border)
            {
                return size;
            }
            else
            {
                return size * (price / supply_border);
            }
        }

        public double ReceiveWithPatent(double amount)
        {
            return owner.Receive(amount);
        }
    }
}
