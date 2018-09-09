using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public class ID
    {
        [JsonProperty] Reference<FIS> ref_fis;
        public int id { get; private set; }

        public ID(FIS fis)
        {
            ref_fis = new Reference<FIS>(fis);
            id = ((IDGenerator)fis.GetModule(ModuleType.IDGenerator)).Generate();
        }

        public ID(int id)
        {
            this.id = id;
        }

        protected void SetFIS(FIS fis)
        {
            ref_fis = new Reference<FIS>(fis);
        }

        [JsonConstructor]
        public ID(Reference<FIS> ref_fis, int id)
        {
            this.ref_fis = ref_fis;
            this.id = id;
        }

        public FIS GetFIS()
        {
            return ref_fis.Get();
        }

        protected Notifications GetNotifications()
        {
            return (Notifications)GetFIS().GetModule(ModuleType.Notifications);
        }

        protected Date GetDate()
        {
            return (Date)GetFIS().GetModule(ModuleType.Date);
        }

        protected Rand GetRand()
        {
            return (Rand)GetFIS().GetModule(ModuleType.Rand);
        }

        protected GameData GetGameData()
        {
            return (GameData)GetFIS().GetModule(ModuleType.GameData);
        }

        protected FactoryCounter GetFactoryCounter()
        {
            return (FactoryCounter)GetFIS().GetModule(ModuleType.FactoryCounter);
        }

        protected MarketPrice GetMarketPrice()
        {
            return (MarketPrice)GetFIS().GetModule(ModuleType.MarketPrice);
        }

        protected ResourcesCaches GetResourcesCaches()
        {
            return (ResourcesCaches)GetFIS().GetModule(ModuleType.ResourcesCaches);
        }

        protected Bank GetBank()
        {
            return (Bank)GetFIS().GetModule(ModuleType.Bank);
        }

        protected Humans GetHumans()
        {
            return (Humans)GetFIS().GetModule(ModuleType.Humans);
        }

        protected Companies GetCompanies()
        {
            return (Companies)GetFIS().GetModule(ModuleType.Companies);
        }

        protected Calculator GetCalculator()
        {
            return (Calculator)GetFIS().GetModule(ModuleType.Calculator);
        }

        public virtual void Setup() { }

        public virtual void AddReference(ReferenceBuilder builder)
        {
            builder.AddReference(id, this);
        }

        public virtual void SetReferences(ReferenceBuilder builder)
        {
            ref_fis.SetReference(builder);
        }

        public virtual void AfterConstructor() { }
    }
}
