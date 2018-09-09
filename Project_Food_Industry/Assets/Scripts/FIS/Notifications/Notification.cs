using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;

namespace Food_Industry
{
    public class Notification
    {
        Subject<Parameters> subject = new Subject<Parameters>();

        public UniRx.IObservable<Parameters> GetSubject()
        {
            return subject;
        }

        public void OnNext(Parameters value)
        {
            subject.OnNext(value);
        }
    }
}
