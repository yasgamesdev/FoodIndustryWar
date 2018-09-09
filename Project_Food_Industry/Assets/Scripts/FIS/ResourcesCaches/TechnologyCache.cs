using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class TechnologyCache : ResourcesCache
    {
        [JsonProperty] Dictionary<TechnologyType, List<Reference<Technology>>> technologies;

        public TechnologyCache(FIS fis) : base(ResourceType.Technology, fis)
        {
            technologies = new Dictionary<TechnologyType, List<Reference<Technology>>>();
            foreach(TechnologyType tec_type in Enum.GetValues(typeof(TechnologyType)))
            {
                technologies.Add(tec_type, new List<Reference<Technology>>());
            }
        }

        [JsonConstructor]
        public TechnologyCache(Dictionary<TechnologyType, List<Reference<Technology>>> technologies, ResourceType type, Reference<FIS> ref_fis, int id) : base(type, ref_fis, id)
        {
            this.technologies = technologies;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            technologies.Values.ToList().ForEach(x => x.ForEach(y => y.SetReference(builder)));
        }

        public override void AddResource(Resource resource)
        {
            Technology technology = (Technology)resource;
            technologies[technology.tec_type].Add(new Reference<Technology>(technology));

            GetNotifications().Notify(NotificationType.AddTechnology, technology);
        }

        public override void RemoveResource(Resource resource)
        {
            Technology technology = (Technology)resource;
            technologies[technology.tec_type].RemoveAll(x => x.Get() == technology);
        }

        public int GetGeneration(TechnologyType tec_type)
        {
            return technologies[tec_type].Count;
        }

        public Technology GetLatestTechnology(TechnologyType tec_type)
        {
            return technologies[tec_type].Last().Get();
        }

        public List<Technology> GetLatestTechnologies(Food food)
        {
            return technologies.Where(x => food.tec_types.Contains(x.Key)).Select(x => x.Value.Last().Get()).ToList();
        }
    }
}
