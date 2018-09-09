using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class LandCache : ResourcesCache
    {
        [JsonProperty] Reference<Land>[,] lands;

        public LandCache(FIS fis) : base(ResourceType.Technology, fis)
        {
            lands = new Reference<Land>[MEnv.x_size, MEnv.y_size];
        }

        [JsonConstructor]
        public LandCache(Reference<Land>[,] lands, ResourceType type, Reference<FIS> ref_fis, int id) : base(type, ref_fis, id)
        {
            this.lands = lands;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            lands.Cast<Reference<Land>>().ToList().ForEach(x => x.SetReference(builder));
        }

        public override void AddResource(Resource resource)
        {
            Land land = (Land)resource;
            lands[land.pos.x, land.pos.y] = new Reference<Land>(land);
        }

        public override void RemoveResource(Resource resource)
        {
            Land land = (Land)resource;
            lands[land.pos.x, land.pos.y] = null;
        }

        public Land[,] GetLands()
        {
            Land[,] ret = new Land[lands.GetLength(0), lands.GetLength(1)];
            for (int x = 0; x < lands.GetLength(0); x++)
            {
                for (int y = 0; y < lands.GetLength(0); y++)
                {
                    ret[x, y] = lands[x, y].Get();
                }
            }
            return ret;
        }

        public Land GetFreeLand()
        {
            List<Land> free_lands = lands.Cast<Reference<Land>>().Where(x => x.Get().type == LandType.Soil && x.Get().GetFactory() == null).Select(x => x.Get()).ToList();
            return free_lands[GetRand().Next(free_lands.Count)];
        }

        public bool IsPossibleConstruct(Pos pos)
        {
            return lands[pos.x, pos.y].Get().IsPossibleConstruct();
        }

        public List<Land> GetPossibleConstructLands()
        {
            return lands.Cast<Reference<Land>>().Where(x => GetResourcesCaches().IsPossibleConstruct(x.Get().pos)).Select(x => x.Get()).ToList();
        }
    }
}
