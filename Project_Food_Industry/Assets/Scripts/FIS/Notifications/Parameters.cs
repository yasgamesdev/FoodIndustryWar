using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Parameters
    {
        object[] parameters;

        public Parameters(params object[] parameters)
        {
            this.parameters = parameters;
        }

        public T Get<T>(int index)
        {
            return (T)parameters[index];
        }
    }
}
