using Newtonsoft.Json;
using PCGSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Rand : ID
    {
        Pcg pcg;
        [JsonProperty] UInt32 upper, lower;

        public Rand(int seed, FIS fis) : base(fis)
        {
            pcg = new Pcg(seed);

            SetState();
        }

        void SetState()
        {
            upper = (UInt32)(pcg.GetState() >> 32);
            lower = (UInt32)((pcg.GetState() << 32) >> 32);
        }

        [JsonConstructor]
        public Rand(UInt32 upper, UInt32 lower, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            pcg = new Pcg();

            if (!MEnv.is_reset_seed)
            {
                ulong upper64 = upper;
                ulong lower64 = lower;
                pcg.SetState((upper64 << 32) + lower64);
            }

            SetState();
        }

        public int Next()
        {
            var ret = pcg.Next();
            SetState();
            return ret;
        }

        public int Next(int maxExclusive)
        {
            var ret = pcg.Next(maxExclusive);
            SetState();
            return ret;
        }

        public int Next(int minInclusive, int maxExclusive)
        {
            var ret = pcg.Next(minInclusive, maxExclusive);
            SetState();
            return ret;
        }

        public int[] NextInts(int count, int minInclusive, int maxExclusive)
        {
            var ret = pcg.NextInts(count, minInclusive, maxExclusive);
            SetState();
            return ret;
        }

        public double[] NextDoubles(int count)
        {
            var ret = pcg.NextDoubles(count);
            SetState();
            return ret;
        }

        public double[] NextDoubles(int count, double minInclusive, double maxExclusive)
        {
            var ret = pcg.NextDoubles(count, minInclusive, maxExclusive);
            SetState();
            return ret;
        }

        public double NextDouble()
        {
            var ret = pcg.NextDouble();
            SetState();
            return ret;
        }

        public double NextDouble(double maxExclusive)
        {
            var ret = pcg.NextDouble(maxExclusive);
            SetState();
            return ret;
        }

        public double NextDouble(double minInclusive, double maxExclusive)
        {
            var ret = pcg.NextDouble(minInclusive, maxExclusive);
            SetState();
            return ret;
        }

        public double NextGaussian(double mu, double sigma)
        {
            double u1 = 1.0 - pcg.NextDouble();
            double u2 = 1.0 - pcg.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2);
            var ret = mu + sigma * randStdNormal;
            SetState();
            return ret;
        }

        public double NextGaussianGreaterThanZero(double mu, double sigma)
        {
            double u1 = 1.0 - pcg.NextDouble();
            double u2 = 1.0 - pcg.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2);
            var ret = mu + sigma * randStdNormal;
            if (ret < 0)
            {
                ret = 0;
            }
            SetState();
            return ret;
        }
    }
}
