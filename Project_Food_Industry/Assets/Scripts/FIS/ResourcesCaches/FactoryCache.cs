using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;

namespace Food_Industry
{
    public class FactoryCache : ResourcesCache
    {
        [JsonProperty] Dictionary<int, List<Reference<Factory>>> supply_factories;
        [JsonProperty] Dictionary<int, List<Reference<Factory>>> demand_factories;

        public FactoryCache(FIS fis) : base(ResourceType.Factory, fis)
        {
            supply_factories = new Dictionary<int, List<Reference<Factory>>>();
            demand_factories = new Dictionary<int, List<Reference<Factory>>>();
        }

        [JsonConstructor]
        public FactoryCache(Dictionary<int, List<Reference<Factory>>> supply_factories, Dictionary<int, List<Reference<Factory>>> demand_factories, ResourceType type, Reference<FIS> ref_fis, int id) : base(type, ref_fis, id)
        {
            this.supply_factories = supply_factories;
            this.demand_factories = demand_factories;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            supply_factories.Values.ToList().ForEach(x => x.ForEach(y => y.SetReference(builder)));
            demand_factories.Values.ToList().ForEach(x => x.ForEach(y => y.SetReference(builder)));
        }

        //public override void AfterConstructor()
        //{
        //    GetNotifications().GetSubject(NotificationType.RemoveFood).Subscribe<Parameters>(x => {
        //        Food food = x.Get<Food>(0);
        //        RemoveFactories(food);
        //    });
        //}

        void AddItem(bool supply, int product_id, Factory factory)
        {
            Dictionary<int, List<Reference<Factory>>> tar = supply ? supply_factories : demand_factories;

            if (!tar.ContainsKey(product_id))
            {
                tar.Add(product_id, new List<Reference<Factory>>());
            }

            tar[product_id].Add(new Reference<Factory>(factory));
        }

        void RemoveItem(bool supply, int product_id, Factory factory)
        {
            Dictionary<int, List<Reference<Factory>>> tar = supply ? supply_factories : demand_factories;

            tar[product_id].RemoveAll(x => x.Get() == factory);

            if(tar[product_id].Count == 0)
            {
                tar.Remove(product_id);
            }
        }

        public override void AddResource(Resource resource)
        {
            Factory factory = (Factory)resource;

            //supply
            AddItem(true, factory.GetSupplyProductID(), factory);

            //demand
            factory.GetDemandProductID().ForEach(x => AddItem(false, x, factory));

            if(factory.GetLand() != null)
            {
                factory.GetLand().SetFactory(factory);
                //GetNotifications().Notify(NotificationType.ChangeLand, factory.GetLand());
            }
        }

        public override void RemoveResource(Resource resource)
        {
            Factory factory = (Factory)resource;

            //supply
            RemoveItem(true, factory.GetSupplyProductID(), factory);

            //demand
            factory.GetDemandProductID().ForEach(x => RemoveItem(false, x, factory));

            if (factory.GetLand() != null)
            {
                factory.GetLand().ClearFactory();
                //GetNotifications().Notify(NotificationType.ChangeLand, factory.GetLand());
            }
        }

        public List<Factory> GetDemandFactories(int product_id)
        {
            if (demand_factories.ContainsKey(product_id))
            {
                //List<Factory> factories = demand_factories[product_id].Select(x => x.Get()).ToList();
                //if(factories.Any(x => x.GetOwner() != GetBank()))
                //{
                //    return factories.Where(x => x.GetOwner() != GetBank()).ToList();
                //}
                //else
                //{
                //    return factories;
                //}
                return demand_factories[product_id].Select(x => x.Get()).ToList();
            }
            else
            {
                return new List<Factory>();
            }
        }

        public List<Factory> GetSupplyFactories(int product_id)
        {
            if (supply_factories.ContainsKey(product_id))
            {
                //List<Factory> factories = supply_factories[product_id].Select(x => x.Get()).ToList();
                //if (factories.Any(x => x.GetOwner() != GetBank()))
                //{
                //    return factories.Where(x => x.GetOwner() != GetBank()).ToList();
                //}
                //else
                //{
                //    return factories;
                //}
                return supply_factories[product_id].Select(x => x.Get()).ToList();
            }
            else
            {
                return new List<Factory>();
            }
        }

        public bool IsContainFactoryConstructedToday()
        {
            return supply_factories.Values.ToList().Any(x => x.Any(y => y.Get().construct_date == GetDate().GetCurrentDateTime()));
        }

        //public void RemoveFactories(Food food)
        //{
        //    List<Factory> remove_factories = supply_factories[food.output_product_id].Select(x => x.Get()).ToList();
        //    remove_factories.ForEach(x => x.GetOwner().RemoveResource(ResourceType.Factory, x));
        //}

        public void ResetHumansFactory()
        {
            HumansFactory factory = (HumansFactory)supply_factories[(int)ProductType.Labor].First(x => x.Get() is HumansFactory).Get();

            demand_factories.Values.ToList().ForEach(y => y.RemoveAll(z => z.Get() == factory));
            factory.GetDemandProductID().ForEach(x => AddItem(false, x, factory));
        }

        public List<FoodFactory> GetConstructedFoodFactory()
        {
            List<FoodFactory> ret = new List<FoodFactory>();

            foreach(var item in supply_factories.Where(x => x.Key != (int)ProductType.None && x.Key != (int)ProductType.Labor))
            {
                item.Value.Where(x => x.Get().GetOwner() != GetBank()).ToList().ForEach(x => ret.Add((FoodFactory)x.Get()));
            }

            return ret;
        }
    }
}
