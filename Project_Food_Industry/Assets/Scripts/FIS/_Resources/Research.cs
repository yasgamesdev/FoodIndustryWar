using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Research : Resource
    {
        public ResearchType research_type { get; private set; }
        public TechnologyType technology_type { get; private set; }
        public FoodRequirement food_requirement { get; private set; }

        public Research(Agent owner) : base(owner)
        {
            research_type = ResearchType.None;
            food_requirement = new FoodRequirement("", new List<ProductType>());
        }

        [JsonConstructor]
        public Research(ResearchType research_type, TechnologyType technology_type, FoodRequirement food_requirement, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.research_type = research_type;
            this.technology_type = technology_type;
            this.food_requirement = food_requirement;
        }

        public void SetResearchTechnology(TechnologyType technology_type)
        {
            research_type = ResearchType.Technology;
            this.technology_type = technology_type;
        }

        public void SetResearchFood(FoodRequirement food_requirement)
        {
            research_type = ResearchType.Food;
            this.food_requirement = food_requirement;
        }

        public void DoResearch()
        {
            if(research_type == ResearchType.Technology)
            {
                ResearchTechnology();
            }
            else if(research_type == ResearchType.Food)
            {
                ResearchFood();
            }
        }

        void ResearchTechnology()
        {
            double success_probability = GetOwner().GetTechnologySuccessProbability(technology_type);

            if(success_probability >= GetRand().NextDouble())
            {
                InventTechnology();

                research_type = ResearchType.None;
            }
        }

        void InventTechnology()
        {
            Technology cur_tec = GetResourcesCaches().GetLatestTechnology(technology_type);
            //double output = cur_tec.output_magnification * (GetGameData().GetTechnologyData(technology_type).min_magnification + Math.Abs(GetRand().NextGaussian(0.0, GetGameData().GetTechnologyData(technology_type).sigma)));
            double output = cur_tec.output_magnification + GetGameData().GetTechnologyData(technology_type).min_magnification + Math.Abs(GetRand().NextGaussian(0.0, GetGameData().GetTechnologyData(technology_type).sigma));
            Technology new_tec = new Technology(technology_type, cur_tec.generation + 1, output, GetOwner());

            GetOwner().AddResource(ResourceType.Technology, new_tec);
        }

        void ResearchFood()
        {
            double success_probability = GetOwner().GetFoodSuccessProbability();

            if (success_probability >= GetRand().NextDouble())
            {
                GetFIS().AddFood(food_requirement, GetOwner());

                research_type = ResearchType.None;
            }
        }

        public void Cancel()
        {
            research_type = ResearchType.None;
        }
    }

    public enum ResearchType
    {
        None,
        Technology,
        Food,
    }

    public class FoodRequirement
    {
        public string food_name { get; private set; }
        public List<ProductType> mats { get; private set; }

        public FoodRequirement(string food_name, List<ProductType> mats)
        {
            this.food_name = food_name;
            this.mats = mats;
        }

        public InputRates GetInputRates(Rand rand)
        {
            List<InputRate> inputs = new List<InputRate>();
            foreach(ProductType type in mats)
            {
                inputs.Add(new InputRate((int)type, rand.NextDouble()));
            }
            return new InputRates(inputs.ToArray());
        }
    }
}
