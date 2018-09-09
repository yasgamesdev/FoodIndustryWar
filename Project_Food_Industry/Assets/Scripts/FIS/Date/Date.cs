using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace Food_Industry
{
    public class Date : ID
    {
        [JsonProperty] DateTime date;

        public Date(FIS fis) : base(fis)
        {
            date = MEnv.init_date;
        }

        [JsonConstructor]
        public Date(DateTime date, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.date = date;
        }

        public void AddOneDay()
        {
            date = date.AddDays(1);
            GetNotifications().Notify(NotificationType.ChangeDate, date);
        }

        public DateTime GetCurrentDateTime()
        {
            return date;
        }

        public bool IsNewYear()
        {
            return (date.Month == 1 && date.Day == 1);
        }

        public bool LastDayOfTheYear()
        {
            return (date.Month == 12 && date.Day == 31);
        }
    }
}
