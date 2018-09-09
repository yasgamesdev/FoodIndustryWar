using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Loan : Resource
    {
        [JsonProperty] Reference<Debt> ref_debt;
        public double principal { get; private set; }
        public double annual_interest_rate { get; private set; }
        public DateTime borrow_date { get; private set; }

        public Loan(double principal, double annual_interest_rate, Agent owner) : base(owner)
        {
            this.principal = principal;
            this.annual_interest_rate = annual_interest_rate;
            borrow_date = GetDate().GetCurrentDateTime();
        }

        public void SetDebt(Debt debt)
        {
            ref_debt = new Reference<Debt>(debt);
        }

        [JsonConstructor]
        public Loan(Reference<Debt> ref_debt, double principal, double annual_interest_rate, DateTime borrow_date, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.ref_debt = ref_debt;
            this.principal = principal;
            this.annual_interest_rate = annual_interest_rate;
            this.borrow_date = borrow_date;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            ref_debt.SetReference(builder);
        }

        public Debt GetDebt()
        {
            return ref_debt.Get();
        }

        public bool IsPayday()
        {
            return borrow_date.AddDays(MEnv.loan_base_term) == GetDate().GetCurrentDateTime();
        }

        public DateTime GetPayday()
        {
            return borrow_date.AddDays(MEnv.loan_base_term);
        }

        public double GetTotalPayAmount()
        {
            return principal * (1.0 + annual_interest_rate);
        }

        public double ReceivePayment(Agent entity)
        {
            return GetOwner().Receive(entity.Pay(GetTotalPayAmount()));
        }

        public override double GetAssets()
        {
            return principal;
        }

        public override double GetFinalAssetValue()
        {
            return GetTotalPayAmount();
        }
    }
}
