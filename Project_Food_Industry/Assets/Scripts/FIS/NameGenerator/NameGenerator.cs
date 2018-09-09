using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public static class NameGenerator
    {
        static bool inited = false;

        static List<string> food_names = new List<string>();

        static void Init()
        {
            inited = true;

            LoadFile(food_names, "food_name_list");
        }

        static void LoadFile(List<string> list, string filename)
        {
#if CONSOLE
            string[] lines = File.ReadAllText(filename).Split('\n');
#else
            string[] lines = ((UnityEngine.TextAsset)UnityEngine.Resources.Load(System.IO.Path.GetFileNameWithoutExtension(filename))).text.Split('\n');
#endif

            foreach (string line in lines)
            {
                string name = line.Split(' ')[0];
                if(name != "")
                {
                    list.Add(name);
                }
            }
        }

        public static string GetFoodName(Rand rand)
        {
            if (!inited)
            {
                Init();
            }

            return food_names[rand.Next(food_names.Count)];
        }
    }
}
