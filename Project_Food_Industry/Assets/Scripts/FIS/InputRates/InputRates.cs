using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class InputRates
    {
        [JsonProperty] List<InputRate> inputs;

        public InputRates(InputRate[] inputs)
        {
            this.inputs = new List<InputRate>(inputs);
        }

        [JsonConstructor]
        public InputRates(List<InputRate> inputs)
        {
            this.inputs = inputs;
        }

        public List<int> GetAllProductID()
        {
            return inputs.Select(x => x.product_id).ToList();
        }

        public double GetInputRate(int product_id)
        {
            return inputs.Where(x => x.product_id == product_id).Sum(x => x.rate);
        }

        public double GetCost(Prices prices, int product_id, double price)
        {
            return inputs.Sum(x => x.rate * prices.GetPrice(x.product_id, product_id, price));
        }

        public double GetInputAmount(double total_output, int product_id)
        {
            return total_output * GetInputRate(product_id);
        }

        public List<InputRate> GetInputRates()
        {
            return inputs;
        }

        public static InputRates operator +(InputRates left, InputRates right)
        {
            Dictionary<int, double> dic = new Dictionary<int, double>();

            List<InputRate> list = new List<InputRate>();
            list.AddRange(left.GetInputRates());
            list.AddRange(right.GetInputRates());

            foreach (InputRate mat in list)
            {
                if(!dic.ContainsKey(mat.product_id))
                {
                    dic.Add(mat.product_id, 0.0);
                }
                dic[mat.product_id] += mat.rate;
            }

            List<InputRate> result = new List<InputRate>();
            foreach(var item in dic)
            {
                if(item.Value > 0)
                {
                    result.Add(new InputRate(item.Key, item.Value));
                }
            }
            return new InputRates(result.ToArray());
        }
    }
}
