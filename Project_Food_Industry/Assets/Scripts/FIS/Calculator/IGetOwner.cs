using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    interface IGetOwner
    {
        Agent GetOwner();
        double ReceiveWithPatent(double amount);
    }
}
