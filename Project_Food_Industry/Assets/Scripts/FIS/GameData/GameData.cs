using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class GameData : ID
    {
        Dictionary<ResourceType, Dictionary<Enum, object>> data;

        public GameData(FIS fis) : base(fis)
        {
            LoadData();
        }

        [JsonConstructor]
        public GameData(Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            LoadData();
        }

        void LoadData()
        {
            data = new Dictionary<ResourceType, Dictionary<Enum, object>>();
            data.Add(ResourceType.Food, FoodData.GetFoodData());
            data.Add(ResourceType.Technology, TechnologyData.GetTechnologyData());
        }

        T GetData<T>(ResourceType type, Enum e_num)
        {
            return (T)data[type][e_num];
        }

        public FoodData GetFoodData(ProductType type)
        {
            return GetData<FoodData>(ResourceType.Food, type);
        }

        public TechnologyData GetTechnologyData(TechnologyType type)
        {
            return GetData<TechnologyData>(ResourceType.Technology, type);
        }
    }
}
