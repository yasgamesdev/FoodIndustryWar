using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class Technology : Invention
    {
        public TechnologyType tec_type { get; private set; }
        public int generation { get; private set; }
        public double output_magnification { get; private set; }

        public Technology(TechnologyType tec_type, int generation, double output_magnification, Agent owner) : base(owner)
        {
            this.tec_type = tec_type;
            this.generation = generation;
            this.output_magnification = output_magnification;
        }

        [JsonConstructor]
        public Technology(TechnologyType tec_type, int generation, double output_magnification, DateTime invention_date, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(invention_date, ref_owner, ref_fis, id)
        {
            this.tec_type = tec_type;
            this.generation = generation;
            this.output_magnification = output_magnification;
        }
    }
}
