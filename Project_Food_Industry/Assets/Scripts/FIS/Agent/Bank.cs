using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using UniRx;

namespace Food_Industry
{
    public class Bank : Agent
    {
        public double total_print_num { get; private set; }

        public Bank(FIS fis) : base(fis)
        {
            total_print_num = 0;
            SetName("Bank");
        }

        [JsonConstructor]
        public Bank(double total_print_num, string name, AgentResources resources, AgentState state, Reference<FIS> ref_fis, int id) : base(name, resources, state, ref_fis, id)
        {
            this.total_print_num = total_print_num;
        }

        protected override AgentResources GetAgentResources()
        {
            return new BankResources(this);
        }

        internal double PrintMoney(object init_company_money)
        {
            throw new NotImplementedException();
        }

        public override void AfterConstructor()
        {
            base.AfterConstructor();

            GetNotifications().GetSubject(NotificationType.AddFood).Subscribe<Parameters>(x => {
                Food food = x.Get<Food>(0);
                if(food.output_product_id != (int)ProductType.Labor)
                {
                    FoodFactory factory = new FoodFactory(food, 1, null, 0, this);
                    GetBank().AddResource(ResourceType.Factory, factory);
                }
            });
        }

        public override void Setup()
        {
            base.Setup();

            SetupLands();
            SetupTechnologies();
            SetupFoods();

            Receive(PrintMoney(MEnv.init_bank_money));

            AddNoneFactory();
        }

        void SetupLands()
        {
            RootObject root = GetRootObject();

            for (int y = 0; y < root.height; y++)
            {
                for (int x = 0; x < root.width; x++)
                {
                    Land land = new Land((LandType)root.layers[0].data[x + y * root.width], new Pos(x, (root.height - 1) - y), this);
                    AddResource(ResourceType.Land, land);
                }
            }
        }

        RootObject GetRootObject()
        {
            string filename = MEnv.map_filename;

#if CONSOLE
            string json = File.ReadAllText(filename);
#else
            string json = ((UnityEngine.TextAsset)UnityEngine.Resources.Load(System.IO.Path.GetFileNameWithoutExtension(filename))).text;
#endif
            return JsonConvert.DeserializeObject<RootObject>(json);
        }

        void SetupTechnologies()
        {
            foreach(TechnologyType tec_type in Enum.GetValues(typeof(TechnologyType)))
            {
                Technology technology = new Technology(tec_type, GetResourcesCaches().GetGeneration(tec_type), 0.0, this);
                AddResource(ResourceType.Technology, technology);
            }
        }

        void SetupFoods()
        {
            foreach(ProductType type in Enum.GetValues(typeof(ProductType)))
            {
                FoodData food_data = GetGameData().GetFoodData(type);
                Food food = new Food(food_data.output_product_id, food_data.classification, food_data.food_name, food_data.inputs, food_data.base_output, food_data.tec_types, this);
                AddResource(ResourceType.Food, food);
            }

            SetupPizza();
            SetupBread();
        }

        void SetupPizza()
        {
            Food food = new Food(GetResourcesCaches().GenerateProductID(), FoodClassification.Dish, "ピザ",  new InputRates(new InputRate[] {
                new InputRate((int)ProductType.Dough, 1.0),
                new InputRate((int)ProductType.Cheese, 1.0),
                new InputRate((int)ProductType.Tomato, 1.0)}),
            1.0, Lib.GetDishTechnologyType(), this);
            AddResource(ResourceType.Food, food);
        }

        void SetupBread()
        {
            Food food = new Food(GetResourcesCaches().GenerateProductID(), FoodClassification.Dish, "パン", new InputRates(new InputRate[] {
                new InputRate((int)ProductType.Dough, 1.0),
                new InputRate((int)ProductType.Salt, 0.2),}),
            1.0, Lib.GetDishTechnologyType(), this);
            AddResource(ResourceType.Food, food);
        }

        public double PrintMoney(double amount)
        {
            total_print_num += amount;
            return amount;
        }

        public void OpenAccount(Agent client)
        {
            Deposit account = new Deposit(this);
            Passbook book = new Passbook(account, client);

            resources.AddResource(ResourceType.Deposit, account);
            client.AddResource(ResourceType.Passbook, book);
        }

        public double GetInterestRate(double principal)
        {
            return ((BankResources)resources).GetInterestRate(principal);
        }

        public double GetIndividualInterestRate(double principal, Agent owner)
        {
            return ((BankResources)resources).GetIndividualInterestRate(principal, owner);
        }

        public void BorrowMoney(Agent client, double principal)
        {
            ((BankResources)resources).BorrowMoney(client, principal);
        }

        public void SupplyMoney(Agent target)
        {
            double amount = ((BankResources)resources).GetSupplyMoneyAmount();
            target.Receive(Pay(amount));
        }
    }
}
