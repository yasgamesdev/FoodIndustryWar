using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class Skill : Resource
    {
        [JsonProperty] Dictionary<SKillType, int> levels;
        [JsonProperty] double total_cost;

        public Skill(Agent owner) : base(owner)
        {
            levels = new Dictionary<SKillType, int>();

            foreach(SKillType type in Enum.GetValues(typeof(SKillType)))
            {
                levels.Add(type, 0);
            }

            total_cost = 0;
        }

        [JsonConstructor]
        public Skill(Dictionary<SKillType, int> levels, double total_cost, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.levels = levels;
            this.total_cost = total_cost;
        }

        public bool IsPossibleLevelUp(SKillType type)
        {
            switch(type)
            {
                case SKillType.reduce_construction_cost:
                    return levels[SKillType.reduce_construction_cost] <= 99 && GetOwner().GetM1() >= GetLevelUpCost(type);
                case SKillType.increases_research_probability:
                    return levels[SKillType.increases_research_probability] <= 99 && GetOwner().GetM1() >= GetLevelUpCost(type);
                default:
                    return false;
            }
        }

        public double GetLevelUpCost(SKillType type)
        {
            return GetFIS().GetM1() / GetCompanies().GetAllCompanies().Count * MEnv.level_up_cost_rate;
        }

        public void LevelUp(SKillType type)
        {
            if(IsPossibleLevelUp(type))
            {
                total_cost += GetLevelUpCost(type);
                GetBank().Receive(GetOwner().Pay(GetLevelUpCost(type)));
                levels[type]++;
            }
        }

        public int GetLevel(SKillType type)
        {
            return levels[type];
        }

        public override double GetAssets()
        {
            return total_cost;
        }
    }

    public enum SKillType
    {
        reduce_construction_cost,
        increases_research_probability,
    }
}
