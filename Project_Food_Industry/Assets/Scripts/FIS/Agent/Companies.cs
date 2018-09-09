using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Companies : ID
    {
        [JsonProperty] List<Company> companies;

        public Companies(CompanyColor color, int cpu_num, FIS fis) : base(fis)
        {
            companies = new List<Company>();

            List<CompanyColor> selected = new List<CompanyColor>();
            for(int i=0; i< cpu_num + 1; i++)
            {
                CompanyColor new_color = i == 0 ? color : ColorGenerator.GetColor(selected, GetRand());
                selected.Add(new_color);

                companies.Add(new Company(i == 0, new_color, fis));
            }
        }

        [JsonConstructor]
        public Companies(List<Company> companies, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.companies = companies;
        }

        public override void AfterConstructor()
        {
            base.AfterConstructor();
            companies.ForEach(x => x.AfterConstructor());
        }

        public override void Setup()
        {
            companies.ForEach(x => x.Setup());
        }

        public override void AddReference(ReferenceBuilder builder)
        {
            base.AddReference(builder);
            companies.ForEach(x => x.AddReference(builder));
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            companies.ForEach(x => x.SetReferences(builder));
        }

        public double GetM0()
        {
            return companies.Sum(x => x.GetM0());
        }

        public double GetM1()
        {
            return companies.Sum(x => x.GetM1());
        }

        public List<Company> GetAllCompanies()
        {
            return companies;
        }

        public void AddCompany(Company company)
        {
            companies.Add(company);

            GetNotifications().Notify(NotificationType.AddCompany, company);
        }

        public void RemoveCompany(Company company)
        {
            companies.Remove(company);
            company.Destroy();

            GetNotifications().Notify(NotificationType.RemoveCompany, company);
        }

        public Company GetPlayerCompany()
        {
            return companies.FirstOrDefault(x => x.is_player);
        }

        public void Act()
        {
            if (!GetResourcesCaches().IsContainFactoryConstructedToday() && GetResourcesCaches().GetPlans().Count == 0)
            {
                List<Company> not_player = companies.Where(x => !x.is_player).ToList();
                if (not_player.Count > 0)
                {
                    Company company = not_player[GetRand().Next(not_player.Count)];
                    company.Act();
                }
            }
        }

        //public void Act()
        //{
        //    if(!GetResourcesCaches().IsContainFactoryConstructedToday())
        //    {
        //        List<Company> not_player = companies.Where(x => !x.is_player).ToList();
        //        Lib.Shuffle(not_player, GetRand());
        //        foreach (Company company in not_player)
        //        {
        //            if (GetResourcesCaches().GetPlans().Count == 0)
        //            {
        //                company.Act();
        //            }
        //        }
        //    }
        //}
    }
}
