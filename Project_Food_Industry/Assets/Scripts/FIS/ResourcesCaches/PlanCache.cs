using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class PlanCache : ResourcesCache
    {
        [JsonProperty] List<Reference<Plan>> plans;

        public PlanCache(FIS fis) : base(ResourceType.Plan, fis)
        {
            plans = new List<Reference<Plan>>();
        }

        [JsonConstructor]
        public PlanCache(List<Reference<Plan>> plans, ResourceType type, Reference<FIS> ref_fis, int id) : base(type, ref_fis, id)
        {
            this.plans = plans;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            plans.ForEach(x => x.SetReference(builder));
        }

        public override void AddResource(Resource resource)
        {
            Plan plan= (Plan)resource;
            plans.Add(new Reference<Plan>(plan));

            GetNotifications().Notify(NotificationType.AddPlan, plan);
        }

        public override void RemoveResource(Resource resource)
        {
            Plan plan = (Plan)resource;
            plans.RemoveAll(x => x.Get() == plan);

            GetNotifications().Notify(NotificationType.RemovePlan, plan);
        }

        public List<Plan> GetPlans()
        {
            return plans.ConvertAll(x => x.Get());
        }

        public bool IsPossibleConstruct(Pos pos)
        {
            return !plans.Any(x => x.Get().GetLand().pos.x == pos.x && x.Get().GetLand().pos.y == pos.y);
        }
    }
}
