using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class FoodCache : ResourcesCache
    {
        [JsonProperty] Dictionary<int, Reference<Food>> foods;
        [JsonProperty] int id_counter;

        public FoodCache(FIS fis) : base(ResourceType.Food, fis)
        {
            foods = new Dictionary<int, Reference<Food>>();
            id_counter = Enum.GetValues(typeof(ProductType)).Length;
        }

        [JsonConstructor]
        public FoodCache(Dictionary<int, Reference<Food>> foods, int id_counter, ResourceType type, Reference<FIS> ref_fis, int id) : base(type, ref_fis, id)
        {
            this.foods = foods;
            this.id_counter = id_counter;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            foods.Values.ToList().ForEach(x => x.SetReference(builder));
        }

        public override void AddResource(Resource resource)
        {
            Food food = (Food)resource;
            foods.Add(food.output_product_id, new Reference<Food>(food));

            GetNotifications().Notify(NotificationType.AddFood, food);
        }

        public override void RemoveResource(Resource resource)
        {
            Food food = (Food)resource;
            foods.Remove(food.output_product_id);

            GetNotifications().Notify(NotificationType.RemoveFood, food);
        }

        public List<int> GetMarketProductID()
        {
            return foods.Where(x => x.Key != (int)ProductType.None).Select(x => x.Key).ToList();
        }

        public List<int> GetConsumerProductID()
        {
            return foods.Where(x => x.Key >= Enum.GetValues(typeof(ProductType)).Length).Select(x => x.Key).ToList();
        }

        public int GenerateProductID()
        {
            return id_counter++;
        }

        public List<Food> GetAllFood()
        {
            return foods.Values.ToList().ConvertAll(x => (x.Get()));
        }

        public List<Food> GetConsumerFoods()
        {
            return foods.Values.ToList().Where(x => x.Get().output_product_id >= Enum.GetValues(typeof(ProductType)).Length).Select(x => x.Get()).ToList();
        }

        public string GetFoodName(int product_id)
        {
            return foods.Values.First(x => x.Get().output_product_id == product_id).Get().food_name;
        }
    }
}
