using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;

namespace Food_Industry
{
    public class Notifications : ID
    {
        Dictionary<NotificationType, Notification> notifications = new Dictionary<NotificationType, Notification>();

        public Notifications(FIS fis) : base(fis)
        {
            SetNotifications();
        }

        [JsonConstructor]
        public Notifications(Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            SetNotifications();
        }

        void SetNotifications()
        {
            foreach(NotificationType type in Enum.GetValues(typeof(NotificationType)))
            {
                notifications.Add(type, new Notification());
            }
        }

        public void Notify(NotificationType type, params object[] parameters)
        {
            notifications[type].OnNext(new Parameters(parameters));
        }

        public UniRx.IObservable<Parameters> GetSubject(NotificationType type)
        {
            return notifications[type].GetSubject();
        }
    }
}
