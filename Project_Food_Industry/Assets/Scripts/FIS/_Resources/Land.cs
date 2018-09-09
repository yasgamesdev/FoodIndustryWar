using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Land : Resource
    {
        public LandType type { get; private set; }
        public Pos pos { get; private set; }
        [JsonProperty] Reference<Factory> ref_factory;

        public Land(LandType type, Pos pos, Agent owner) : base(owner)
        {
            this.type = type;
            this.pos = pos;
        }

        [JsonConstructor]
        public Land(LandType type, Pos pos, Reference<Factory> ref_factory, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.type = type;
            this.pos = pos;
            this.ref_factory = ref_factory;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            if(ref_factory != null)
            {
                ref_factory.SetReference(builder);
            }
        }

        public Factory GetFactory()
        {
            if(ref_factory == null)
            {
                return null;
            }
            else
            {
                return ref_factory.Get();
            }
        }

        public void SetFactory(Factory factory)
        {
            ref_factory = new Reference<Factory>(factory);
            GetNotifications().Notify(NotificationType.ChangeLand, this);
        }

        public void ClearFactory()
        {
            ref_factory = null;
            GetNotifications().Notify(NotificationType.ChangeLand, this);
        }

        public bool IsPossibleConstruct()
        {
            return type == LandType.Soil && (GetFactory() == null || (GetFactory() is FoodFactory && ((FoodFactory)GetFactory()).GetFood().output_product_id != (int)ProductType.None));
        }
    }

    public struct Pos
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public enum LandType
    {
        None,
        Soil,
        Grass,
        Subway = 26,
        Tree = 27,
        LittleFlowerBed = 59,
    }
}
