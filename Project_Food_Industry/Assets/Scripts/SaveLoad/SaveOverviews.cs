using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Food_Industry;

public class SaveOverviews
{
    public string version;
    public List<SaveOverview> overviews;

    public SaveOverviews()
    {
        version = MEnv.version;
        overviews = new List<SaveOverview>();
    }

    [JsonConstructor]
    public SaveOverviews(string version, List<SaveOverview> overviews)
    {
        this.version = version;
        this.overviews = overviews;
    }

    public void Add(DateTime date, int difficulty, double money)
    {
        overviews.Add(new SaveOverview(overviews.Count, date, difficulty, money));
    }

    public void OverWrite(int index, DateTime date, int difficulty, double money)
    {
        overviews[index] = new SaveOverview(index, date, difficulty, money);
    }
}