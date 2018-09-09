using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace Food_Industry
{
    public class CompanyColor
    {
        public int r { get; private set; }
        public int g { get; private set; }
        public int b { get; private set; }
        public int a { get; private set; }

        public float float_r { get { return r / 255.0f; } }
        public float float_g { get { return g / 255.0f; } }
        public float float_b { get { return b / 255.0f; } }
        public float float_a { get { return a / 255.0f; } }

        public string color_name { get; private set; }

        public CompanyColor(int r, int g, int b, int a, string color_name)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            this.color_name = color_name;
        }

        public string GetColorNameForTag()
        {
            return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
        }
    }
}
