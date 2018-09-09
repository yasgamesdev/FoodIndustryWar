using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;

namespace Food_Industry
{
    public class Humans : Agent
    {
        public double size { get; private set; }

        public Humans(double size, FIS fis) : base(fis)
        {
            this.size = size;
            SetName("Humans");
        }

        [JsonConstructor]
        public Humans(double size, string name, AgentResources resources, AgentState state, Reference<FIS> ref_fis, int id) : base(name, resources, state, ref_fis, id)
        {
            this.size = size;
        }

        protected override AgentResources GetAgentResources()
        {
            return new CommonResources(this);
        }

        public override void AfterConstructor()
        {
            base.AfterConstructor();

            GetNotifications().GetSubject(NotificationType.AddFood).Subscribe<Parameters>(x => {
                Food food = x.Get<Food>(0);
                if (resources.GetResources(ResourceType.Factory).Any(y => y is HumansFactory))
                {
                    GetResourcesCaches().ResetHumansFactory();
                }
            });

            GetNotifications().GetSubject(NotificationType.RemoveFood).Subscribe<Parameters>(x => {
                Food food = x.Get<Food>(0);
                if (resources.GetResources(ResourceType.Factory).Any(y => y is HumansFactory))
                {
                    GetResourcesCaches().ResetHumansFactory();
                }
            });
        }

        public override void Setup()
        {
            base.Setup();

            GetBank().OpenAccount(this);
            Receive(GetBank().PrintMoney(MEnv.init_humans_money * size));

            HumansFactory factory = new HumansFactory(this);
            AddResource(ResourceType.Factory, factory);

            AddNoneFactory();
        }

        public void AddSize(double amount)
        {
            size += amount;
            GetNotifications().Notify(NotificationType.ChangePopulation, size);
        }
    }
}
