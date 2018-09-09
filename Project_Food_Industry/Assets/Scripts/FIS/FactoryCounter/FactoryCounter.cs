using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace Food_Industry
{
    public class FactoryCounter : ID
    {
        [JsonProperty] int count;

        public FactoryCounter(FIS fis) : base(fis)
        {
            count = 0;
        }

        [JsonConstructor]
        public FactoryCounter(int count, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.count = count;
        }

        public double GetSize()
        {
            return ((double)count / MEnv.max_factory_count) + 1.0;
        }

        public double GenerateSize()
        {
            return ((double)count++ / MEnv.max_factory_count) + 1.0;
        }
    }
}
