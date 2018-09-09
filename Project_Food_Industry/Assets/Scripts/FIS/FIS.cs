using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;
using System.Diagnostics;

namespace Food_Industry
{
    public class FIS : ID
    {
        public int difficulty { get; private set; }
        [JsonProperty] Dictionary<ModuleType, ID> modules;

        public FIS(int difficulty, int seed, CompanyColor color, int cpu_num) : base(0)
        {
            this.difficulty = difficulty;
            SetFIS(this);

            modules = new Dictionary<ModuleType, ID>();

            modules.Add(ModuleType.IDGenerator, new IDGenerator(this));
            modules.Add(ModuleType.Notifications, new Notifications(this));
            modules.Add(ModuleType.Date, new Date(this));
            modules.Add(ModuleType.Rand, new Rand(seed, this));
            modules.Add(ModuleType.GameData, new GameData(this));
            modules.Add(ModuleType.FactoryCounter, new FactoryCounter(this));
            modules.Add(ModuleType.MarketPrice, new MarketPrice(this));
            modules.Add(ModuleType.ResourcesCaches, new ResourcesCaches(this));
            modules.Add(ModuleType.Bank, new Bank(this));
            modules.Add(ModuleType.Humans, new Humans(MEnv.init_humans_size, this));
            modules.Add(ModuleType.Companies, new Companies(color, cpu_num, this));
            modules.Add(ModuleType.Calculator, new Calculator(this));

            AfterConstructor();

            Setup();
        }

        [JsonConstructor]
        public FIS(int difficulty, Dictionary<ModuleType, ID> modules, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.difficulty = difficulty;
            this.modules = modules;

            ReferenceBuilder builder = new ReferenceBuilder();
            AddReference(builder);
            SetReferences(builder);

            AfterConstructor();
        }

        public override void Setup()
        {
            base.Setup();
            modules.Values.ToList().ForEach(x => x.Setup());
        }

        public override void AddReference(ReferenceBuilder builder)
        {
            base.AddReference(builder);
            modules.Values.ToList().ForEach(x => x.AddReference(builder));
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            modules.Values.ToList().ForEach(x => x.SetReferences(builder));
        }

        public override void AfterConstructor()
        {
            base.AfterConstructor();
            modules.Values.ToList().ForEach(x => x.AfterConstructor());
        }

        public ID GetModule(ModuleType type)
        {
            return modules[type];
        }

        public T GetModule<T>(ModuleType type)
        {
            return (T)(object)modules[type];
        }

        public UniRx.IObservable<Parameters> GetSubject(NotificationType type)
        {
            return GetNotifications().GetSubject(type);
        }

        public DateTime GetCurrentDateTime()
        {
            return GetDate().GetCurrentDateTime();
        }

        public double GetPopulation()
        {
            return GetHumans().size;
        }

        public List<Agent> GetAllAgent()
        {
            List<Agent> ret = new List<Agent>();
            
            ret.Add(GetBank());
            ret.Add(GetHumans());
            ret.AddRange(GetCompanies().GetAllCompanies());

            return ret;
        }

        public Company GetPlayerCompany()
        {
            return GetCompanies().GetPlayerCompany();
        }

        public Land[,] GetLands()
        {
            return GetResourcesCaches().GetLands();
        }

        public List<Plan> GetPlans()
        {
            return GetResourcesCaches().GetPlans();
        }

        public CalculatorState GetState()
        {
            return GetCalculator().GetState();
        }

        public void Calculate()
        {
            if(GetState() == CalculatorState.Stop)
            {
                GetCalculator().Calculate();
            }
        }

        public void Stop()
        {
            GetCalculator().Stop();
        }

        public void Merge()
        {
            GetMarketPrice().Merge(GetCalculator().GetCalculatedPrices());
        }

        public void Produce()
        {
            GetCalculator().Produce();
        }

        public List<Food> GetAllFood()
        {
            return GetResourcesCaches().GetAllFood();
        }

        public List<TechnologyType> GetTechnologyTypes(TechnologyClassification classification)
        {
            List<TechnologyType> types = new List<TechnologyType>();
            foreach(TechnologyType type in Enum.GetValues(typeof(TechnologyType)))
            {
                if(GetGameData().GetTechnologyData(type).classification == classification)
                {
                    types.Add(type);
                }
            }
            return types;
        }

        public FoodFactoryCore CreateFoodFactoryCore(Food food, Agent owner)
        {
            List<Technology> technologies = GetResourcesCaches().GetLatestTechnologies(food);

            double output = food.base_output;
            technologies.ForEach(x => output += x.output_magnification);

            double newbase = Math.Pow(0.5, 1 / (MEnv.half_output_labor_num * GetFactoryCounter().GetSize()));

            return new FoodFactoryCore(food.output_product_id, output, newbase, food.inputs, GetMarketPrice().GetPrices(), new Patent(owner, GetBank(), food, technologies), owner);
        }

        public double GetM1()
        {
            return GetHumans().GetM1() + GetCompanies().GetM1();
        }

        public double GetConstructCost(Food food, double size, Agent owner)
        {
            int reduce_construct_cost_level = owner.GetSkill().GetLevel(SKillType.reduce_construction_cost);
            return GetMarketPrice().GetPrice((int)ProductType.Labor) * MEnv.construction_population * size * (1.0 - 0.005 * reduce_construct_cost_level);
            //List<Technology> latest_tecnologies = GetResourcesCaches().GetLatestTechnologies(food);
            //double output = food.base_output;
            //latest_tecnologies.ForEach(x => output += x.output_magnification);
            //double rate = Math.Sqrt(output / food.base_output);
            //return GetMarketPrice().GetPrice((int)ProductType.Labor) * MEnv.construction_population * rate;
        }

        public double GetDestructCost(FoodFactory factory)
        {
            return GetMarketPrice().GetPrice((int)ProductType.Labor) * MEnv.destruction_population * factory.size;
            //double output = factory.GetOutput();
            //double rate = Math.Sqrt(output / factory.GetFood().base_output);
            //return GetMarketPrice().GetPrice((int)ProductType.Labor) * MEnv.destruction_population * rate;
        }

        public bool IsPossibleConstruct(Pos pos)
        {
            return GetResourcesCaches().IsPossibleConstruct(pos);
        }

        public double GetTotalConstructCost(Food food, Pos pos, Agent owner)
        {
            Land land = GetResourcesCaches().GetLands()[pos.x, pos.y];

            if(land.GetFactory() == null)
            {
                return GetConstructCost(food, GetFactoryCounter().GetSize(), owner);
            }
            else
            {
                if(owner == ((FoodFactory)land.GetFactory()).GetOwner())
                {
                    return GetConstructCost(food, GetFactoryCounter().GetSize(), owner);
                }
                else
                {
                    return GetConstructCost(food, GetFactoryCounter().GetSize(), owner) + GetDestructCost((FoodFactory)land.GetFactory());
                }
            }
        }

        public void Plan(Pos pos, Food food, Agent owner)
        {
            double construct_cost = GetConstructCost(food, GetFactoryCounter().GetSize(), owner);

            if (IsPossibleConstruct(pos) && owner.GetM1() >= construct_cost)
            {
                Land land = GetResourcesCaches().GetLands()[pos.x, pos.y];

                if (land.GetFactory() != null && owner != land.GetFactory().GetOwner())
                {
                    double destruct_cost = GetDestructCost((FoodFactory)land.GetFactory());
                    land.GetFactory().GetOwner().Receive(owner.Pay(destruct_cost));
                }

                GetBank().Receive(owner.Pay(construct_cost));
                owner.AddResource(ResourceType.Plan, new Plan(food, land, GetFactoryCounter().GenerateSize(), construct_cost, owner));
            }
        }

        public double GetInterestRate(double principal)
        {
            return GetBank().GetInterestRate(principal);
        }

        public double GetIndividualInterestRate(double principal, Agent owner)
        {
            return GetBank().GetIndividualInterestRate(principal, owner);
        }

        public double GetMaxBorrowAmount(Agent owner)
        {
            //double final_asset_value = owner.GetFinalAssetValue();
            //return final_asset_value > 0 ? final_asset_value : 0;
            double amount = owner.GetAssets() - (2.0 * owner.GetLiabilities());
            return amount > 0 ? amount : 0;
        }

        public void BorrowMoney(Agent client, double principal)
        {
            GetBank().BorrowMoney(client, principal);
        }

        public List<Debt> GetDebts(Agent owner)
        {
            return owner.GetDebts();
        }

        public void PayBack(Debt debt, Agent owner)
        {
            owner.PayBack(debt);
        }

        public bool IsGameOver()
        {
            return GetPlayerCompany().state == AgentState.Bankrupt;
        }

        void PayDebts()
        {
            GetAllAgent().ForEach(x => x.PayDebt());
        }

        void Act()
        {
            GetCompanies().Act();
        }

        void IncreasePopulation()
        {
            double labor_price = GetMarketPrice().GetPrice((int)ProductType.Labor);
            int produt_id = (int)ProductType.Labor;
            double supply_amount = GetResourcesCaches().GetSupplyFactories(produt_id).Sum(x => x.GetISupplyFunc(GetMarketPrice().GetPrices()).GetSupplyAmount(produt_id, labor_price));
            if(supply_amount >= GetHumans().size * 0.7)
            {
                int size = GetRand().Next(MEnv.increase_num);
                GetHumans().AddSize(size);
                GetHumans().Receive(GetBank().PrintMoney(size * MEnv.init_humans_money));
            }
        }

        void SupplyMoney()
        {
            GetBank().SupplyMoney(GetHumans());
        }

        void Construct()
        {
            foreach (Plan plan in GetResourcesCaches().GetPlans())
            {
                if(plan.GetLand().GetFactory() != null)
                {
                    plan.GetLand().GetOwner().RemoveResource(ResourceType.Factory, plan.GetLand().GetFactory());
                }

                Agent owner = plan.GetOwner();
                FoodFactory factory = new FoodFactory(plan.GetFood(), plan.size, plan.GetLand(), plan.construct_cost, owner);
                owner.AddResource(ResourceType.Factory, factory);

                owner.RemoveResource(ResourceType.Plan, plan);
            }
        }

        void Destruct()
        {
            List<FoodFactory> factories = GetResourcesCaches().GetConstructedFoodFactory();

            if(factories.Count > MEnv.max_factory_count)
            {
                int remove_num = factories.Count - MEnv.max_factory_count;

                factories = factories.OrderBy(x => x.construct_date).ToList();
                for(int i=0; i< remove_num; i++)
                {
                    factories[i].GetOwner().RemoveResource(ResourceType.Factory, factories[i]);
                }
            }
        }

        void SetResearch()
        {
            //List<TechnologyType> types = Enum.GetValues(typeof(TechnologyType)).Cast<TechnologyType>().ToList();
            //double max_success_probability = types.Max(x => GetBank().GetSuccessProbability(x));
            //List<TechnologyType> max_types = types.Where(x => GetBank().GetSuccessProbability(x) == max_success_probability).ToList();
            //TechnologyType max_type = max_types[GetRand().Next(max_types.Count)];

            //foreach(Company company in GetCompanies().GetAllCompanies().Where(x => !x.is_player))
            //{
            //    if(GetRand().Next(4) == 0)
            //    {

            //    }
            //    else
            //    {
            //        company.GetResearch().SetResearchTechnology(max_type);
            //    }
            //}

            List<TechnologyType> types = Enum.GetValues(typeof(TechnologyType)).Cast<TechnologyType>().ToList();
            List<ProductType> input_types = Enum.GetValues(typeof(ProductType)).Cast<ProductType>().ToList().Where(x => x != ProductType.None && x != ProductType.Labor).ToList();

            foreach (Company company in GetCompanies().GetAllCompanies().Where(x => !x.is_player))
            {
                if (GetRand().Next(MEnv.cpu_food_research_rate) == 0)
                {
                    string food_name = NameGenerator.GetFoodName(GetRand());
                    int input_num = GetRand().Next(1, 6);
                    List<ProductType> mats = new List<ProductType>();
                    for(int i=0; i<input_num; i++)
                    {
                        mats.Add(input_types[GetRand().Next(input_types.Count)]);
                    }
                    mats = mats.Distinct().ToList();

                    company.GetResearch().SetResearchFood(new FoodRequirement(food_name, mats));
                }
                else
                {
                    company.GetResearch().SetResearchTechnology(types[GetRand().Next(types.Count)]);
                }
            }
        }

        void Research()
        {
            GetCompanies().GetAllCompanies().ForEach(x => x.GetResearch().DoResearch());
        }

        public void AddFood(FoodRequirement food_requirement, Agent owner)
        {
            if(GetResourcesCaches().GetConsumerFoods().Count >= MEnv.max_food_count)
            {
                List<Food> foods = GetResourcesCaches().GetConsumerFoods();
                Food food = foods[GetRand().Next(foods.Count)];
                food.GetOwner().RemoveResource(ResourceType.Food, food);
            }

            Food new_food = new Food(GetResourcesCaches().GenerateProductID(), FoodClassification.Dish, food_requirement.food_name, food_requirement.GetInputRates(GetRand()), 1.0, Lib.GetDishTechnologyType(), owner);
            owner.AddResource(ResourceType.Food, new_food);
        }

        public void Next()
        {
            if (GetState() == CalculatorState.Stop)
            {
                GetNotifications().Notify(NotificationType.BeginNext, null);

                BeforeDay();

                GetDate().AddOneDay();

                NextDay();

                GetNotifications().Notify(NotificationType.EndNext, null);

                if(IsGameOver())
                {
                    GetNotifications().Notify(NotificationType.GameOver, null);
                }
            }
        }

        void BeforeDay()
        {
            Merge();
            Produce();

            PayDebts();
            //ProcessBankrupt();

            Act();

            IncreasePopulation();

            SupplyMoney();

            //ConsoleShow();
            CheckAllMoney();
        }

        void NextDay()
        {
            Construct();
            Destruct();

            SetResearch();
            Research(); //need after Construct();
        }

        public void ConsoleShow()
        {
            foreach (int product_id in GetResourcesCaches().GetMarketProductID())
            {
                if (product_id != (int)ProductType.Labor)
                {
                    List<IDemandFunc> demands = new List<IDemandFunc>();
                    List<ISupplyFunc> supplies = new List<ISupplyFunc>();
                    double price = GetMarketPrice().GetPrice(product_id);

                    GetResourcesCaches().GetDemandFactories(product_id).ForEach(x => demands.Add(x.GetIDemandFunc(product_id, GetMarketPrice().GetPrices())));
                    double demand_amount = demands.Sum(x => x.GetDemandAmount(product_id, price));

                    GetResourcesCaches().GetSupplyFactories(product_id).ForEach(x => supplies.Add(x.GetISupplyFunc(GetMarketPrice().GetPrices())));
                    double supply_amount = supplies.Sum(x => x.GetSupplyAmount(product_id, price));

                    double labor_num = supplies.Sum(x => ((FoodFactoryCore)x).GetLaborNum(product_id, price));
                    double net_income = supplies.Sum(x => ((FoodFactoryCore)x).GetDailyNetIncome());
                    double margin = supplies.Average(x => ((FoodFactoryCore)x).GetNetProfitMargin());

                    Lib.Debug($"{(ProductType)product_id}: demand:{demand_amount:F0} -> supply:{supply_amount:F0} labor:{labor_num:F0} price:{price:F2}, netincome:{net_income:F2}, margin:{margin:F2}");
                }
                else
                {
                    List<IDemandFunc> demands = new List<IDemandFunc>();
                    List<ISupplyFunc> supplies = new List<ISupplyFunc>();
                    double price = GetMarketPrice().GetPrice(product_id);

                    GetResourcesCaches().GetDemandFactories(product_id).ForEach(x => demands.Add(x.GetIDemandFunc(product_id, GetMarketPrice().GetPrices())));
                    double demand_amount = demands.Sum(x => x.GetDemandAmount(product_id, price));

                    GetResourcesCaches().GetSupplyFactories(product_id).ForEach(x => supplies.Add(x.GetISupplyFunc(GetMarketPrice().GetPrices())));
                    double supply_amount = supplies.Sum(x => x.GetSupplyAmount(product_id, price));

                    Lib.Debug($"{(ProductType)product_id}: demand:{demand_amount:F0} -> supply:{supply_amount:F0} price:{price:F2}");
                }
            }
        }

        public void CheckAllMoney()
        {
            Lib.Debug($"M0:{GetAllAgent().Sum(x => x.GetM0())}, TotalPrintNum:{GetBank().total_print_num}");
        }
    }
}
