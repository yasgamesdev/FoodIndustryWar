using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public interface ISupplyFunc
    {
        double GetSupplyAmount(int product_id, double price);
    }
}
