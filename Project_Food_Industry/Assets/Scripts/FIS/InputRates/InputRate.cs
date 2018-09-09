using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class InputRate
    {
        public int product_id { get; private set; }
        public double rate { get; private set; }

        public InputRate(int product_id, double rate)
        {
            this.product_id = product_id;
            this.rate = rate;
        }
    }
}
