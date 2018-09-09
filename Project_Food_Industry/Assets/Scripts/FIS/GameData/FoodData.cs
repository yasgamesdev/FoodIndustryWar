using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class FoodData
    {
        public int output_product_id { get; private set; }
        public FoodClassification classification { get; private set; }
        public string food_name { get; private set; }
        public InputRates inputs { get; private set; }
        public double base_output { get; private set; }
        public List<TechnologyType> tec_types { get; private set; }

        public FoodData(ProductType product_type, FoodClassification classification, string food_name, InputRate[] inputs, double base_output, TechnologyType[] tec_types)
        {
            output_product_id = (int)product_type;
            this.classification = classification;
            this.food_name = food_name;
            this.inputs = new InputRates(inputs);
            this.base_output = base_output;
            this.tec_types = new List<TechnologyType>(tec_types);
        }

        public static Dictionary<Enum, object> GetFoodData()
        {
            Dictionary<Enum, object> data = new Dictionary<Enum, object>();

            data.Add(ProductType.None, new FoodData(ProductType.None, FoodClassification.None, "なし", new InputRate[] { }, 1.0, new TechnologyType[] { }));
            data.Add(ProductType.Labor, new FoodData(ProductType.Labor, FoodClassification.None, "労働者", new InputRate[] { }, 1.0, new TechnologyType[] { }));
            data.Add(ProductType.Wheat, new FoodData(ProductType.Wheat, FoodClassification.Grain, "小麦", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Salt, new FoodData(ProductType.Salt, FoodClassification.Seasoning, "塩", new InputRate[] { }, 1.0, new TechnologyType[] {TechnologyType.Salt }));
            data.Add(ProductType.Sugar, new FoodData(ProductType.Sugar, FoodClassification.Seasoning, "砂糖", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Sugar}));
            data.Add(ProductType.Dough, new FoodData(ProductType.Dough, FoodClassification.ProcessedFood, "パン生地", new InputRate[] {
                new InputRate((int)ProductType.Wheat, 1.0),
                new InputRate((int)ProductType.Salt, 0.2),
                new InputRate((int)ProductType.Sugar, 0.2),
            }, 1.0, new TechnologyType[] { TechnologyType.BreadMachine}));
            data.Add(ProductType.Milk, new FoodData(ProductType.Milk, FoodClassification.Milk, "牛乳", new InputRate[] { }, 1.0, new TechnologyType[] {TechnologyType.Feed, TechnologyType.Domesticated }));
            data.Add(ProductType.Cheese, new FoodData(ProductType.Cheese, FoodClassification.Milk, "チーズ", new InputRate[] {
                new InputRate((int)ProductType.Milk, 1.0),
            }, 1.0, new TechnologyType[] { TechnologyType.Cheese}));
            data.Add(ProductType.Tomato, new FoodData(ProductType.Tomato, FoodClassification.Vegetable, "トマト", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));

            //Grain
            data.Add(ProductType.Rice, new FoodData(ProductType.Rice, FoodClassification.Grain, "米", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Corn, new FoodData(ProductType.Corn, FoodClassification.Grain, "トウモロコシ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Soy, new FoodData(ProductType.Soy, FoodClassification.Grain, "大豆", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));

            //Vegetable
            data.Add(ProductType.Potato, new FoodData(ProductType.Potato, FoodClassification.Vegetable, "じゃがいも", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.SweetPotato, new FoodData(ProductType.SweetPotato, FoodClassification.Vegetable, "サツマイモ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Cabbage, new FoodData(ProductType.Cabbage, FoodClassification.Vegetable, "キャベツ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Radish, new FoodData(ProductType.Radish, FoodClassification.Vegetable, "だいこん", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Onion, new FoodData(ProductType.Onion, FoodClassification.Vegetable, "たまねぎ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Leek, new FoodData(ProductType.Leek, FoodClassification.Vegetable, "ねぎ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Spinach, new FoodData(ProductType.Spinach, FoodClassification.Vegetable, "ほうれん草", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Lettuce, new FoodData(ProductType.Lettuce, FoodClassification.Vegetable, "レタス", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Carrot, new FoodData(ProductType.Carrot, FoodClassification.Vegetable, "にんじん", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Pumpkin, new FoodData(ProductType.Pumpkin, FoodClassification.Vegetable, "かぼちゃ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Cucumber, new FoodData(ProductType.Cucumber, FoodClassification.Vegetable, "きゅうり", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Eggplant, new FoodData(ProductType.Eggplant, FoodClassification.Vegetable, "ナス", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));

            //Milk
            data.Add(ProductType.Yogurt, new FoodData(ProductType.Yogurt, FoodClassification.Milk, "ヨーグルト", new InputRate[] {
                new InputRate((int)ProductType.Milk, 1.0),
            }, 1.0, new TechnologyType[] {TechnologyType.FoodProcessingMachinery }));
            data.Add(ProductType.Cream, new FoodData(ProductType.Cream, FoodClassification.Milk, "生クリーム", new InputRate[] {
                new InputRate((int)ProductType.Milk, 1.0),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));
            data.Add(ProductType.Butter, new FoodData(ProductType.Butter, FoodClassification.Milk, "バター", new InputRate[] {
                new InputRate((int)ProductType.Milk, 1.0),
                new InputRate((int)ProductType.Salt, 0.2),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));

            //Meat
            data.Add(ProductType.Beef, new FoodData(ProductType.Beef, FoodClassification.Meat, "牛肉", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Domesticated}));
            data.Add(ProductType.Pork, new FoodData(ProductType.Pork, FoodClassification.Meat, "豚肉", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Domesticated }));
            data.Add(ProductType.Chicken, new FoodData(ProductType.Chicken, FoodClassification.Meat, "鶏肉", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Domesticated }));
            data.Add(ProductType.Ham, new FoodData(ProductType.Ham, FoodClassification.Meat, "ハム", new InputRate[] {
                new InputRate((int)ProductType.Pork, 1.0),
                new InputRate((int)ProductType.Salt, 0.5),
            }, 1.0, new TechnologyType[] {TechnologyType.MeatPacking }));
            data.Add(ProductType.Sausage, new FoodData(ProductType.Sausage, FoodClassification.Meat, "ソーセージ", new InputRate[] {
                new InputRate((int)ProductType.Beef, 1.0),
                new InputRate((int)ProductType.Pork, 1.0),
                new InputRate((int)ProductType.Salt, 0.2),
            }, 1.0, new TechnologyType[] { TechnologyType.MeatPacking }));

            //Fish
            data.Add(ProductType.Salmon, new FoodData(ProductType.Salmon, FoodClassification.Fish, "サーモン", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.FishingBoat}));
            data.Add(ProductType.Squid, new FoodData(ProductType.Squid, FoodClassification.Fish, "イカ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.FishingBoat }));
            data.Add(ProductType.Tuna, new FoodData(ProductType.Tuna, FoodClassification.Fish, "まぐろ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.FishingBoat }));
            data.Add(ProductType.Shrimp, new FoodData(ProductType.Shrimp, FoodClassification.Fish, "えび", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.FishingBoat }));
            data.Add(ProductType.Sanma, new FoodData(ProductType.Sanma, FoodClassification.Fish, "サンマ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.FishingBoat }));
            data.Add(ProductType.Crab, new FoodData(ProductType.Crab, FoodClassification.Fish, "カニ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.FishingBoat, TechnologyType.CrabBasket}));
            data.Add(ProductType.Scallops, new FoodData(ProductType.Scallops, FoodClassification.Fish, "ホタテ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.FishingBoat, TechnologyType.Aquaculture }));

            //Seasoning
            data.Add(ProductType.Pepper, new FoodData(ProductType.Pepper, FoodClassification.Seasoning, "コショウ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Vinegar, new FoodData(ProductType.Vinegar, FoodClassification.Seasoning, "酢", new InputRate[] {
                new InputRate((int)ProductType.Liquor, 1.0),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));
            data.Add(ProductType.Liquor, new FoodData(ProductType.Liquor, FoodClassification.Seasoning, "酒", new InputRate[] {
                new InputRate((int)ProductType.Rice, 1.0),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));
            data.Add(ProductType.SoySauce, new FoodData(ProductType.SoySauce, FoodClassification.Seasoning, "醤油", new InputRate[] {
                new InputRate((int)ProductType.Soy, 1.0),
                new InputRate((int)ProductType.Wheat, 1.0),
                new InputRate((int)ProductType.Salt, 0.2),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));
            data.Add(ProductType.Miso, new FoodData(ProductType.Miso, FoodClassification.Seasoning, "味噌", new InputRate[] {
                new InputRate((int)ProductType.Soy, 1.0),
                new InputRate((int)ProductType.Salt, 0.2),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));
            data.Add(ProductType.Mayonnaise, new FoodData(ProductType.Mayonnaise, FoodClassification.Seasoning, "マヨネーズ", new InputRate[] {
                new InputRate((int)ProductType.SaladOil, 1.0),
                new InputRate((int)ProductType.Vinegar, 1.0),
                new InputRate((int)ProductType.Egg, 1.0),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));
            data.Add(ProductType.Ketchup, new FoodData(ProductType.Ketchup, FoodClassification.Seasoning, "ケチャップ", new InputRate[] {
                new InputRate((int)ProductType.Tomato, 1.0),
                new InputRate((int)ProductType.Salt, 0.1),
                new InputRate((int)ProductType.Sugar, 0.1),
                new InputRate((int)ProductType.Vinegar, 0.1),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));

            //Egg
            data.Add(ProductType.Egg, new FoodData(ProductType.Egg, FoodClassification.Egg, "卵", new InputRate[] { }, 1.0, new TechnologyType[] {TechnologyType.Domesticated }));
            data.Add(ProductType.Caviar, new FoodData(ProductType.Caviar, FoodClassification.Egg, "キャビア", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.FishingBoat}));

            //Fruit
            data.Add(ProductType.MandarinOrange, new FoodData(ProductType.MandarinOrange, FoodClassification.Fruit, "みかん", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Apple, new FoodData(ProductType.Apple, FoodClassification.Fruit, "りんご", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));
            data.Add(ProductType.Watermelon, new FoodData(ProductType.Watermelon, FoodClassification.Fruit, "スイカ", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.Agrichemical, TechnologyType.Fertilizer }));

            //Oil
            data.Add(ProductType.OliveOil, new FoodData(ProductType.OliveOil, FoodClassification.Oil, "オリーブオイル", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.OilWringers}));
            data.Add(ProductType.SesameOil, new FoodData(ProductType.SesameOil, FoodClassification.Oil, "ごま油", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.OilWringers }));
            data.Add(ProductType.SaladOil, new FoodData(ProductType.SaladOil, FoodClassification.Oil, "サラダ油", new InputRate[] { }, 1.0, new TechnologyType[] { TechnologyType.OilWringers }));

            //ProcessedFood
            data.Add(ProductType.Consomme, new FoodData(ProductType.Consomme, FoodClassification.ProcessedFood, "コンソメ", new InputRate[] {
                new InputRate((int)ProductType.Beef, 1.0),
                new InputRate((int)ProductType.Chicken, 1.0),
                new InputRate((int)ProductType.Salt, 0.1),
                new InputRate((int)ProductType.Pepper, 0.1),
            }, 1.0, new TechnologyType[] { TechnologyType.FoodProcessingMachinery }));
            data.Add(ProductType.Noodles, new FoodData(ProductType.Noodles, FoodClassification.ProcessedFood, "麺", new InputRate[] {
                new InputRate((int)ProductType.Wheat, 1.0),
                new InputRate((int)ProductType.Salt, 0.1),
            }, 1.0, new TechnologyType[] {TechnologyType.NoodleMachine }));

            return data;
        }
    }
}
