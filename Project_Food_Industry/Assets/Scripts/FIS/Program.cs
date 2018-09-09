using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    class Program
    {
        static void Main(string[] args)
        {
            FIS fis = new FIS(0, 100, ColorGenerator.GetAllColor()[0], MEnv.default_cpu_num);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            string json = JsonConvert.SerializeObject(fis, Formatting.Indented, settings);
            //Console.WriteLine(json);

            FIS de_fis = JsonConvert.DeserializeObject<FIS>(json, settings);

            while(true)
            {
                de_fis.Calculate();

                Console.ReadLine();
                de_fis.Stop();
                while(de_fis.GetState() != CalculatorState.Stop)
                {
                    Task.Delay(1);
                }

                de_fis.Merge();
                de_fis.Produce();

                de_fis.ConsoleShow();
                de_fis.CheckAllMoney();

                foreach (Food food in de_fis.GetAllFood())
                {
                    FoodFactoryCore core = de_fis.CreateFoodFactoryCore(food, de_fis.GetPlayerCompany());
                    Console.WriteLine($"{food.output_product_id}:{core.GetDailyNetIncome()}, {core.GetNetProfitMargin()}");
                }

                for(int i=0; i<10; i++)
                {
                    Console.WriteLine($"{de_fis.GetInterestRate(i * 10000000):P}");
                }

                Console.ReadLine();
            }
        }
    }
}
