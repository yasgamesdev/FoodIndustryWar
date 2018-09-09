using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class ResourcesCache : ID
    {
        public ResourceType type { get; private set; }

        public ResourcesCache(ResourceType type, FIS fis) : base(fis)
        {
            this.type = type;
        }

        [JsonConstructor]
        public ResourcesCache(ResourceType type, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.type = type;
        }

        public virtual void AddResource(Resource resource)
        {

        }

        public virtual void RemoveResource(Resource resource)
        {

        }
    }
}
