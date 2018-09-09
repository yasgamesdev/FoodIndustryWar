using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Food_Industry
{
    public abstract class AgentResources : ID
    {
        [JsonProperty] Reference<Agent> ref_owner;
        [JsonProperty] Dictionary<ResourceType, List<Resource>> resource_dict;

        public AgentResources(Agent owner) : base(owner.GetFIS())
        {
            ref_owner = new Reference<Agent>(owner);

            resource_dict = new Dictionary<ResourceType, List<Resource>>();
            foreach(ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                resource_dict[type] = new List<Resource>();

                if(type == ResourceType.Cash)
                {
                    AddResource(ResourceType.Cash, new Cash(GetOwner()));
                }
                else if(type == ResourceType.Research)
                {
                    AddResource(ResourceType.Research, new Research(GetOwner()));
                }
                else if (type == ResourceType.Skill)
                {
                    AddResource(ResourceType.Skill, new Skill(GetOwner()));
                }
            }
        }

        [JsonConstructor]
        public AgentResources(Reference<Agent> ref_owner, Dictionary<ResourceType, List<Resource>> resource_dict, Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            this.ref_owner = ref_owner;
            this.resource_dict = resource_dict;
        }

        public override void AddReference(ReferenceBuilder builder)
        {
            base.AddReference(builder);
            foreach (var list in resource_dict.Values)
            {
                list.ForEach(x => x.AddReference(builder));
            }
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            ref_owner.SetReference(builder);
            foreach (var list in resource_dict.Values)
            {
                list.ForEach(x => x.SetReferences(builder));
            }
        }

        protected Agent GetOwner()
        {
            return ref_owner.Get();
        }

        public void AddResource(ResourceType type, Resource resource)
        {
            resource_dict[type].Add(resource);
            GetResourcesCaches().AddResource(type, resource);
        }

        public void RemoveResource(ResourceType type, Resource resource)
        {
            resource_dict[type].Remove(resource);
            GetResourcesCaches().RemoveResource(type, resource);

            resource.Destroy();
        }

        public void RemoveFactory(Food food)
        {
            List<Factory> removes = GetResources(ResourceType.Factory).ConvertAll(x => (Factory)x).Where(x => x.GetSupplyProductID() == food.output_product_id).ToList();
            removes.ForEach(x => RemoveResource(ResourceType.Factory, x));
        }

        public List<Resource> GetResources(ResourceType type)
        {
            return resource_dict[type];
        }

        public abstract double Pay(double amount);
        public abstract double Receive(double amount);

        public double GetM0()
        {
            return GetCash().amount;
        }

        public double GetM1()
        {
            return GetM0() + GetResources(ResourceType.Passbook).ConvertAll(x => (Passbook)x).Sum(y => y.GetBalance());
        }

        Cash GetCash()
        {
            return (Cash)GetResources(ResourceType.Cash)[0];
        }

        public void PayDebt()
        {
            List<Debt> debts = GetResources(ResourceType.Debt).ConvertAll(x => (Debt)x).Where(x => x.IsPayday()).ToList();

            debts.ForEach(x => x.Pay());
            debts.ForEach(x => x.GetLoan().GetOwner().RemoveResource(ResourceType.Loan, x.GetLoan()));
            debts.ForEach(x => RemoveResource(ResourceType.Debt, x));
        }

        public Research GetResearch()
        {
            return (Research)GetResources(ResourceType.Research)[0];
        }

        public Skill GetSkill()
        {
            return (Skill)GetResources(ResourceType.Skill)[0];
        }

        public List<Debt> GetDebts()
        {
            return GetResources(ResourceType.Debt).ConvertAll(x => (Debt)x);
        }

        public void PayBack(Debt debt)
        {
            debt.Pay();
            debt.GetLoan().GetOwner().RemoveResource(ResourceType.Loan, debt.GetLoan());
            RemoveResource(ResourceType.Debt, debt);
        }

        public double GetTotalDailyNetIncome()
        {
            return GetResources(ResourceType.Factory).Where(x => x is FoodFactory).ToList().ConvertAll(x => (FoodFactory)x).Sum(x => x.GetFoodFactoryCoreWithMarketPrices().GetDailyNetIncome());
        }

        //void PayDebtForNormal(List<Debt> debts)
        //{
        //    double pay_amount_sum = debts.Sum(x => x.GetPayAmount(x.GetPayYear()));

        //    if((GetOwner() is Human || GetOwner() is Bank) && GetM1() < pay_amount_sum)
        //    {
        //        GetOwner().SetState(EntityState.BankruptProcess);

        //        Lib.Debug($"支払えなかった:{pay_amount_sum}");
        //    }
        //    else
        //    {
        //        ForcedPayDebt(debts);

        //        Lib.Debug($"支払えた:{pay_amount_sum}");
        //    }
        //}

        //void ForcedPayDebt(List<Debt> debts)
        //{
        //    foreach(Debt debt in debts)
        //    {
        //        debt.Pay();

        //        if(debt.IsLastYear(debt.GetPayYear()))
        //        {
        //            debt.GetLoan().GetOwner().RemoveResource(ResourceType.Loan, debt.GetLoan());
        //            RemoveResource(ResourceType.Debt, debt);
        //        }
        //    }
        //}

        //public double GetDepositCapacity(Bank bank)
        //{
        //    if (GetOwner() is Bank || GetOwner() is CentralBank)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        double whole_bank_trust = GetOwner().GetWholeBankTrust();
        //        double individual_bank_trust_sum = GetResources(ResourceType.Passbook).ConvertAll(x => (Passbook)x).Sum(x => x.individual_bank_trust.value);
        //        if (individual_bank_trust_sum == 0)
        //        {
        //            individual_bank_trust_sum = 1.0;
        //        }

        //        Passbook book = GetResources(ResourceType.Passbook).ConvertAll(x => (Passbook)x).First(x => x.GetDeposit().GetOwner() == bank);
        //        double final_trust = whole_bank_trust * (book.individual_bank_trust.value / individual_bank_trust_sum);

        //        return GetM1() * final_trust;
        //    }
        //}

        //public double GetTotalDailyLoanLimit(Type agreement_type)
        //{
        //    return GetResources(ResourceType.Overdraft).ConvertAll(x => (Overdraft)x).Where(x => x.GetAgreement().GetType() == agreement_type).Sum(x => x.daily_loan_limit);
        //}

        //public double GetRemainLoanAmount()
        //{
        //    return GetResources(ResourceType.Overdraft).ConvertAll(x => (Overdraft)x).Sum(x => x.GetRemainLoanAmount());
        //}

        //public virtual void ProcessBankrupt()
        //{
        //}

        //protected void DestoryAgreements()
        //{
        //    List<Agreement> agreements = GetResources(ResourceType.Agreement).ConvertAll(x => (Agreement)x).ToList();
        //    foreach (Agreement agreement in agreements)
        //    {
        //        Debt debt = agreement.GetOverdraft().RemoveOverdraft(agreement);

        //        RemoveResource(ResourceType.Agreement, agreement);
        //        AddResource(ResourceType.Debt, debt);
        //    }
        //}

        //protected void ConfiscateFactories()
        //{
        //    List<Factory> factories = GetResources(ResourceType.Factory).ConvertAll(x => (Factory)x).ToList();
        //    factories.ForEach(x => x.ChangeOwner(ResourceType.Factory, GetGovernment()));
        //}

        //protected void ConfiscateManufacturePatents()
        //{
        //    List<ManufacturePatent> patents = GetResources(ResourceType.ManufacturePatent).ConvertAll(x => (ManufacturePatent)x).ToList();
        //    patents.ForEach(x => x.ChangeOwner(ResourceType.ManufacturePatent, GetGovernment()));
        //}

        //protected void ConfiscateProducts()
        //{
        //    List<Product> products = GetResources(ResourceType.Product).ConvertAll(x => (Product)x).ToList();
        //    products.ForEach(x => x.ChangeOwner(ResourceType.Product, GetGovernment()));
        //}

        //protected void ConfiscateLoans()
        //{
        //    List<Loan> loans = GetResources(ResourceType.Loan).ConvertAll(x => (Loan)x).ToList();
        //    loans.ForEach(x => x.ChangeOwner(ResourceType.Loan, GetGovernment()));
        //}

        public double GetAssets()
        {
            return resource_dict.Sum(x => x.Value.Sum(y => y.GetAssets()));
        }

        public double GetAssets(ResourceType type)
        {
            return resource_dict[type].Sum(x => x.GetAssets());
        }

        public double GetLiabilities()
        {
            return resource_dict.Sum(x => x.Value.Sum(y => y.GetLiabilities()));
        }

        public double GetNetAssets()
        {
            return GetAssets() - GetLiabilities();
        }

        public double GetCapitalAdequacyRatio()
        {
            double assets = GetAssets();

            if (assets == 0)
            {
                return 0;
            }
            else
            {
                return GetNetAssets() / GetAssets();
            }
        }

        public double GetMaxInterestBearingDebt()
        {
            return resource_dict.Sum(x => x.Value.Sum(y => y.GetMaxInterestBearingDebt()));
        }

        public double GetFinalAssetValue()
        {
            return resource_dict.Sum(x => x.Value.Sum(y => y.GetFinalAssetValue()));
        }
    }
}
