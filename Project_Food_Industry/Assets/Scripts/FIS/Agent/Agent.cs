using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace Food_Industry
{
    public abstract class Agent : ID
    {
        public string name { get; private set; }
        [JsonProperty] protected AgentResources resources;
        public AgentState state { get; private set; }

        public Agent(FIS fis) : base(fis)
        {
            name = "";
            resources = GetAgentResources();
            state = AgentState.Normal;
        }

        [JsonConstructor]
        public Agent(string name, AgentResources resources, AgentState state, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.name = name;
            this.resources = resources;
            this.state = state;
        }

        protected void AddNoneFactory()
        {
            Food food = GetResourcesCaches().GetAllFood().First(x => x.output_product_id == (int)ProductType.None);
            Land land = GetResourcesCaches().GetFreeLand();
            FoodFactory factory = new FoodFactory(food, 1, land, 0, this);
            AddResource(ResourceType.Factory, factory);
        }

        public override void AddReference(ReferenceBuilder builder)
        {
            base.AddReference(builder);
            resources.AddReference(builder);
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            resources.SetReferences(builder);
        }

        public override void AfterConstructor()
        {
            GetNotifications().GetSubject(NotificationType.RemoveFood).Subscribe<Parameters>(x => {
                Food food = x.Get<Food>(0);
                resources.RemoveFactory(food);
            });
        }

        public void SetState(AgentState state)
        {
            this.state = state;
        }

        public virtual double Pay(double amount)
        {
            return resources.Pay(amount);
        }

        public virtual double Receive(double amount)
        {
            return resources.Receive(amount);
        }

        public double GetM0()
        {
            return resources.GetM0();
        }

        public double GetM1()
        {
            return resources.GetM1();
        }

        protected void SetName(string name)
        {
            this.name = name;
        }

        public void AddResource(ResourceType type, Resource resource)
        {
            resources.AddResource(type, resource);
        }

        public void RemoveResource(ResourceType type, Resource resource)
        {
            resources.RemoveResource(type, resource);
        }

        public virtual void Destroy() { }

        protected abstract AgentResources GetAgentResources();

        public void PayDebt()
        {
            resources.PayDebt();
        }

        public double GetMaxInterestBearingDebt()
        {
            return resources.GetMaxInterestBearingDebt();
        }

        public double GetAssets()
        {
            return resources.GetAssets();
        }

        public double GetAssets(ResourceType type)
        {
            return resources.GetAssets(type);
        }

        public double GetLiabilities()
        {
            return resources.GetLiabilities();
        }

        public double GetNetAssets()
        {
            return resources.GetNetAssets();
        }

        public virtual double GetFinalAssetValue()
        {
            return resources.GetFinalAssetValue();
        }

        public double GetTechnologySuccessProbability(TechnologyType type)
        {
            int increases_research_probability_level = GetSkill().GetLevel(SKillType.increases_research_probability);

            int next_generation = GetResourcesCaches().GetGeneration(type);
            int base_span = (int)(MEnv.technology_frequency * GetGameData().GetTechnologyData(type).frequency);
            DateTime normal_invention_date = MEnv.init_date.AddDays(base_span * next_generation);

            int sub_days = (GetDate().GetCurrentDateTime() - normal_invention_date).Days;
            return Math.Pow(sub_days >= 0 ? 1.1 : 0.9, Math.Abs(sub_days) / 365.0) * MEnv.technology_probability * GetGameData().GetTechnologyData(type).probability * (1.0 + 0.09 * increases_research_probability_level);
        }

        public double GetFoodSuccessProbability()
        {
            int increases_research_probability_level = GetSkill().GetLevel(SKillType.increases_research_probability);

            return MEnv.food_probability * (1.0 + 0.09 * increases_research_probability_level);
        }

        public Research GetResearch()
        {
            return resources.GetResearch();
        }

        public Skill GetSkill()
        {
            return resources.GetSkill();
        }

        public List<Debt> GetDebts()
        {
            return resources.GetDebts();
        }

        public void PayBack(Debt debt)
        {
            resources.PayBack(debt);
        }

        public double GetTotalDailyNetIncome()
        {
            return resources.GetTotalDailyNetIncome();
        }
    }

    public enum AgentState
    {
        Normal,
        Bankrupt,
    }
}
