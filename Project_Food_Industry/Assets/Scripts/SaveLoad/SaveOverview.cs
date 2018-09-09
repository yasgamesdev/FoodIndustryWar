using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SaveOverview
{
    public int index;
    public DateTime date;
    public int difficulty;
    public double money;

    public SaveOverview(int index, DateTime date, int difficulty, double money)
    {
        this.index = index;
        this.date = date;
        this.difficulty = difficulty;
        this.money = money;
    }
}