using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class IDGenerator : ID
    {
        [JsonProperty] int count;

        public IDGenerator(FIS mes) : base(1)
        {
            SetFIS(mes);

            count = 2;
        }

        [JsonConstructor]
        public IDGenerator(int count, Reference<FIS> ref_mes, int id) : base(ref_mes, id)
        {
            this.count = count;
        }

        public int Generate()
        {
            return count++;
        }
    }
}
