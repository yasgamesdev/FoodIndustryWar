using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
#if CONSOLE
using System.IO.Compression;
#else
using Unity.IO.Compression;
#endif


namespace Food_Industry
{
    public static class Lib
    {
        public static int AddSeed(int seed0, int seed1)
        {
            long seed = seed0 + seed1;

            return (int)seed;
        }

        public static string GetFoodCassificationName(FoodClassification classification)
        {
            switch (classification)
            {
                case FoodClassification.Fish:
                    return "魚";
                case FoodClassification.Meat:
                    return "肉";
                case FoodClassification.Egg:
                    return "卵";
                case FoodClassification.Milk:
                    return "乳製品";
                case FoodClassification.Vegetable:
                    return "野菜";
                case FoodClassification.Fruit:
                    return "果物";
                case FoodClassification.Grain:
                    return "穀物";
                case FoodClassification.Seasoning:
                    return "調味料";
                case FoodClassification.Oil:
                    return "油";
                case FoodClassification.ProcessedFood:
                    return "加工食品";
                default:
                    return "料理";
            }
        }

        public static string GetTechnologyCassificationName(TechnologyClassification classification)
        {
            switch (classification)
            {
                case TechnologyClassification.Agricultural:
                    return "農業";
                case TechnologyClassification.Fishing:
                    return "漁業";
                case TechnologyClassification.Processing:
                    return "加工技術";
                case TechnologyClassification.PlantBreeding:
                    return "植物の品種改良";
                case TechnologyClassification.AnimalBreeding:
                    return "動物の品種改良";
                case TechnologyClassification.Machine:
                    return "機械";
                case TechnologyClassification.EatingOut:
                    return "外食産業";
                default:
                    return "";
            }
        }

        public static string GetTechnologyName(TechnologyType type)
        {
            switch(type)
            {
                case TechnologyType.Agrichemical:
                    return "農薬";
                case TechnologyType.Fertilizer:
                    return "肥料";
                case TechnologyType.Aquaculture:
                    return "養殖";
                case TechnologyType.CrabBasket:
                    return "カニ籠";
                case TechnologyType.FishingBoat:
                    return "漁船";
                case TechnologyType.MeatPacking:
                    return "食肉加工";
                case TechnologyType.Domesticated:
                    return "家畜化";
                case TechnologyType.Salt:
                    return "塩の製法";
                case TechnologyType.Sugar:
                    return "砂糖の製法";
                case TechnologyType.BreadMachine:
                    return "製パン機械";
                case TechnologyType.NoodleMachine:
                    return "製麺機";
                case TechnologyType.OilWringers:
                    return "油搾り器";
                case TechnologyType.FoodProcessingMachinery:
                    return "食品加工機械";
                case TechnologyType.Feed:
                    return "飼料";
                case TechnologyType.Cheese:
                    return "チーズの製法";
                case TechnologyType.Management:
                    return "経営ノウハウ";
                default:
                    return "";
            }
        }

        public static List<TechnologyType> GetDishTechnologyType()
        {
            return new List<TechnologyType>(new TechnologyType[] { TechnologyType.Management });
        }

        public static string ConvertToBase64(object value)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            string json = JsonConvert.SerializeObject(value, Formatting.Indented, settings);
            string base64 = Base64FromStringComp(json);

            return base64;
        }

        public static T ConvertFromBase64<T>(string base64)
        {
            string json = StringFromBase64Comp(base64);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        static string Base64FromStringComp(string st)
        {
            byte[] source = Encoding.UTF8.GetBytes(st);

            MemoryStream ms = new MemoryStream();
            DeflateStream CompressedStream = new DeflateStream(ms, CompressionMode.Compress, true);

            CompressedStream.Write(source, 0, source.Length);
            CompressedStream.Close();

            byte[] destination = ms.ToArray();

            string base64String;
            base64String = System.Convert.ToBase64String(destination, Base64FormattingOptions.InsertLineBreaks);

            return base64String;
        }

        static string StringFromBase64Comp(string st)
        {
            byte[] bs = System.Convert.FromBase64String(st);

            MemoryStream ms = new MemoryStream(bs);
            MemoryStream ms2 = new MemoryStream();

            DeflateStream CompressedStream = new DeflateStream(ms, CompressionMode.Decompress);

            while (true)
            {
                int rb = CompressedStream.ReadByte();
                if (rb == -1)
                {
                    break;
                }
                ms2.WriteByte((byte)rb);
            }

            string result = Encoding.UTF8.GetString(ms2.ToArray());

            return result;
        }

        public static string GetDifficultyName(int difficulty)
        {
            switch(difficulty)
            {
                case 0:
                    return "Very Easy";
                case 1:
                    return "Easy";
                case 2:
                    return "Normal";
                case 3:
                    return "Hard";
                default:
                    return "Very Hard";
            }
        }

        public static void Shuffle<T>(this IList<T> list, Rand rand)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        //public static double GetDepositRate(double deposit_interest_rate)
        //{
        //    return 1.0 - (Math.Pow(MEnv.human_cash_rate_base, deposit_interest_rate * 100));
        //}

        //public static double GetLoanInterestRate(double debt_redemption_year)
        //{
        //    return Math.Max(0.01 * debt_redemption_year, MEnv.min_loan_interest_rate);
        //}

        //public static bool IsOverdraft(int product_id)
        //{
        //    return product_id == (int)ProductAlias.Construction | product_id == (int)ProductAlias.Research || product_id == (int)ProductAlias.Gun;
        //}

        //public static double GetTotalPayAmount(double principal, int pay_num, double annual_interest_rate)
        //{
        //    return principal + ((principal * (1.0 + 1.0 / pay_num)) * (pay_num * 0.5) * annual_interest_rate);
        //}

        public static void Debug(string msg)
        {
#if CONSOLE
            Console.WriteLine(msg);
#else
            UnityEngine.Debug.Log(msg);
#endif
        }

        //public static double GetDebtRedemptionYear(double daily_income, double debt)
        //{
        //    if (daily_income == 0)
        //    {
        //        return MEnv.max_debt_redemption_year;
        //    }
        //    else
        //    {
        //        return (debt / daily_income) / MEnv.pay_span;
        //    }
        //}
    }
}
