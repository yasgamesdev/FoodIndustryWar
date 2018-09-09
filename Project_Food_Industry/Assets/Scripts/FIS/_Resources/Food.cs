using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class Food : Invention
    {
        public int output_product_id { get; private set; }
        public FoodClassification classification { get; private set; }
        public string food_name { get; private set; }
        public InputRates inputs { get; private set; }
        public double base_output { get; private set; }
        public List<TechnologyType> tec_types { get; private set; }

        public Food(int output_product_id, FoodClassification classification, string food_name, InputRates inputs, double base_output, List<TechnologyType> tec_types, Agent owner) : base(owner)
        {
            this.output_product_id = output_product_id;
            this.classification = classification;
            this.food_name = food_name;
            this.inputs = inputs;
            this.base_output = base_output;
            this.tec_types = tec_types;
        }

        [JsonConstructor]
        public Food(int output_product_id, FoodClassification classification, string food_name, InputRates inputs, double base_output, List<TechnologyType> tec_types, DateTime invention_date, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(invention_date, ref_owner, ref_fis, id)
        {
            this.output_product_id = output_product_id;
            this.classification = classification;
            this.food_name = food_name;
            this.inputs = inputs;
            this.base_output = base_output;
            this.tec_types = tec_types;
        }
    }
}
