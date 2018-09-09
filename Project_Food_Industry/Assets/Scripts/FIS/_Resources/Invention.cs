using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class Invention : Resource
    {
        public DateTime invention_date { get; private set; }

        public Invention(Agent owner) : base(owner)
        {
            invention_date = GetDate().GetCurrentDateTime();
        }

        [JsonConstructor]
        public Invention(DateTime invention_date, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.invention_date = invention_date;
        }
    }
}
