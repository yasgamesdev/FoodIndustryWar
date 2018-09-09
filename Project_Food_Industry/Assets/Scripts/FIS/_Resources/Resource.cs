using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public abstract class Resource : ID
    {
        [JsonProperty] Reference<Agent> ref_owner;

        public Resource(Agent owner) : base(owner.GetFIS())
        {
            ref_owner = new Reference<Agent>(owner);
        }

        [JsonConstructor]
        public Resource(Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.ref_owner = ref_owner;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            ref_owner.SetReference(builder);
        }

        public virtual void Destroy() { }

        public Agent GetOwner()
        {
            return ref_owner.Get();
        }

        public void ChangeOwner(ResourceType type, Agent new_owner)
        {
            GetOwner().RemoveResource(type, this);
            ref_owner = new Reference<Agent>(new_owner);
            new_owner.AddResource(type, this);
        }

        public virtual double GetAssets()
        {
            return 0;
        }

        public virtual double GetLiabilities()
        {
            return 0;
        }

        public virtual double GetMaxInterestBearingDebt()
        {
            return 0;
        }

        public virtual double GetFinalAssetValue()
        {
            return 0;
        }
    }
}
