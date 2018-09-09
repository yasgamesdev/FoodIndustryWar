using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Debt : Resource
    {
        [JsonProperty] Reference<Loan> ref_loan;

        public Debt(Loan loan, Agent owner) : base(owner)
        {
            ref_loan = new Reference<Loan>(loan);
            GetLoan().SetDebt(this);
        }

        [JsonConstructor]
        public Debt(Reference<Loan> ref_loan, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.ref_loan = ref_loan;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            ref_loan.SetReference(builder);
        }

        public Loan GetLoan()
        {
            return ref_loan.Get();
        }

        public bool IsPayday()
        {
            return GetLoan().IsPayday();
        }

        public DateTime GetPayday()
        {
            return GetLoan().GetPayday();
        }

        public double GetTotalPayAmount()
        {
            return GetLoan().GetTotalPayAmount();
        }

        public double Pay()
        {
            return GetLoan().ReceivePayment(GetOwner());
        }

        public override double GetLiabilities()
        {
            return GetLoan().principal;
        }

        public override double GetMaxInterestBearingDebt()
        {
            return GetTotalPayAmount();
        }

        public override double GetFinalAssetValue()
        {
            return -GetTotalPayAmount();
        }
    }
}
