using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class ResourcesCaches : ID
    {
        [JsonProperty] Dictionary<ResourceType, ResourcesCache> caches;

        public ResourcesCaches(FIS fis) : base(fis)
        {
            caches = new Dictionary<ResourceType, ResourcesCache>();

            caches.Add(ResourceType.Cash, new ResourcesCache(ResourceType.Cash, fis));
            caches.Add(ResourceType.Passbook, new ResourcesCache(ResourceType.Passbook, fis));
            caches.Add(ResourceType.Deposit, new ResourcesCache(ResourceType.Deposit, fis));
            caches.Add(ResourceType.Debt, new ResourcesCache(ResourceType.Debt, fis));
            caches.Add(ResourceType.Loan, new ResourcesCache(ResourceType.Loan, fis));
            caches.Add(ResourceType.Food, new FoodCache(fis));
            caches.Add(ResourceType.Technology, new TechnologyCache(fis));
            caches.Add(ResourceType.Land, new LandCache(fis));
            caches.Add(ResourceType.Factory, new FactoryCache(fis));
            caches.Add(ResourceType.Plan, new PlanCache(fis));
            caches.Add(ResourceType.Research, new ResourcesCache(ResourceType.Research, fis));
            caches.Add(ResourceType.Skill, new ResourcesCache(ResourceType.Skill, fis));
        }

        [JsonConstructor]
        public ResourcesCaches(Dictionary<ResourceType, ResourcesCache> caches, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.caches = caches;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            caches.Values.ToList().ForEach(x => x.SetReferences(builder));
        }

        public void AddResource(ResourceType type, Resource resource)
        {
            caches[type].AddResource(resource);
        }

        public void RemoveResource(ResourceType type, Resource resource)
        {
            caches[type].RemoveResource(resource);
        }

        public List<int> GetMarketProductID()
        {
            return ((FoodCache)caches[ResourceType.Food]).GetMarketProductID();
        }

        public List<int> GetConsumerProductID()
        {
            return ((FoodCache)caches[ResourceType.Food]).GetConsumerProductID();
        }

        public int GenerateProductID()
        {
            return ((FoodCache)caches[ResourceType.Food]).GenerateProductID();
        }

        public List<Food> GetAllFood()
        {
            return ((FoodCache)caches[ResourceType.Food]).GetAllFood();
        }

        public List<Food> GetConsumerFoods()
        {
            return ((FoodCache)caches[ResourceType.Food]).GetConsumerFoods();
        }

        public string GetFoodName(int product_id)
        {
            return ((FoodCache)caches[ResourceType.Food]).GetFoodName(product_id);
        }

        public int GetGeneration(TechnologyType tec_type)
        {
            return ((TechnologyCache)caches[ResourceType.Technology]).GetGeneration(tec_type);
        }

        public Technology GetLatestTechnology(TechnologyType tec_type)
        {
            return ((TechnologyCache)caches[ResourceType.Technology]).GetLatestTechnology(tec_type);
        }

        public List<Technology> GetLatestTechnologies(Food food)
        {
            return ((TechnologyCache)caches[ResourceType.Technology]).GetLatestTechnologies(food);
        }

        public Land[,] GetLands()
        {
            return ((LandCache)caches[ResourceType.Land]).GetLands();
        }

        public Land GetFreeLand()
        {
            return ((LandCache)caches[ResourceType.Land]).GetFreeLand();
        }

        public List<Factory> GetDemandFactories(int product_id)
        {
            return ((FactoryCache)caches[ResourceType.Factory]).GetDemandFactories(product_id);
        }

        public List<Factory> GetSupplyFactories(int product_id)
        {
            return ((FactoryCache)caches[ResourceType.Factory]).GetSupplyFactories(product_id);
        }

        public bool IsContainFactoryConstructedToday()
        {
            return ((FactoryCache)caches[ResourceType.Factory]).IsContainFactoryConstructedToday();
        }

        public void ResetHumansFactory()
        {
            ((FactoryCache)caches[ResourceType.Factory]).ResetHumansFactory();
        }

        public List<FoodFactory> GetConstructedFoodFactory()
        {
            return ((FactoryCache)caches[ResourceType.Factory]).GetConstructedFoodFactory();
        }

        public List<Plan> GetPlans()
        {
            return ((PlanCache)caches[ResourceType.Plan]).GetPlans();
        }

        public bool IsPossibleConstruct(Pos pos)
        {
            LandCache land_cache = ((LandCache)caches[ResourceType.Land]);
            PlanCache plan_cache = ((PlanCache)caches[ResourceType.Plan]);

            return land_cache.IsPossibleConstruct(pos) && plan_cache.IsPossibleConstruct(pos);
        }

        public List<Land> GetPossibleConstructLands()
        {
            return ((LandCache)caches[ResourceType.Land]).GetPossibleConstructLands();
        }
    }
}
