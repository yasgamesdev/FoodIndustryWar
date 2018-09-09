using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace Food_Industry
{
    public static class ColorGenerator
    {
        static bool inited = false;

        static List<CompanyColor> colors = new List<CompanyColor>();

        static void Init()
        {
            inited = true;

            LoadColors();
        }

        static void LoadColors()
        {
            colors.Add(new CompanyColor(243, 67, 54, 255, "RED"));
            colors.Add(new CompanyColor(63, 81, 181, 255, "INDIGO"));
            colors.Add(new CompanyColor(76, 175, 80, 255, "GREEN"));
            colors.Add(new CompanyColor(254, 152, 0, 255, "ORANGE"));
            colors.Add(new CompanyColor(232, 30, 99, 255, "PINK"));
            colors.Add(new CompanyColor(33, 150, 242, 255, "BLUE"));
            colors.Add(new CompanyColor(139, 194, 74, 255, "LIGHT GREEN"));
            colors.Add(new CompanyColor(254, 87, 34, 255, "DEEP ORANGE"));
            colors.Add(new CompanyColor(3, 169, 243, 255, "LIGHT BLUE"));
            colors.Add(new CompanyColor(204, 219, 57, 255, "LIME"));
            colors.Add(new CompanyColor(121, 85, 72, 255, "BROWN"));
            colors.Add(new CompanyColor(156, 39, 176, 255, "PURPLE"));
            colors.Add(new CompanyColor(0, 188, 211, 255, "CYAN"));
            colors.Add(new CompanyColor(254, 234, 59, 255, "YELLOW"));
            colors.Add(new CompanyColor(158, 158, 158, 255, "GREY"));
            colors.Add(new CompanyColor(103, 58, 183, 255, "DEEP PURPLE"));
            colors.Add(new CompanyColor(0, 150, 136, 255, "TEAL"));
            colors.Add(new CompanyColor(254, 192, 7, 255, "AMBER"));
            colors.Add(new CompanyColor(96, 125, 139, 255, "BLUE GREY"));
        }

        public static List<CompanyColor> GetAllColor()
        {
            if (!inited)
            {
                Init();
            }

            return colors;
        }

        public static CompanyColor GetColor(List<CompanyColor> selected, Rand rand)
        {
            if (!inited)
            {
                Init();
            }

            List<CompanyColor> not_selected = colors.Except(selected).ToList();

            if(not_selected.Count > 0)
            {
                return not_selected[rand.Next(not_selected.Count)];
            }
            else
            {
                return colors[rand.Next(GetAllColor().Count)];
            }
        }
    }
}
