using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Passbook : Resource
    {
        [JsonProperty] Reference<Deposit> ref_deposit;

        public Passbook(Deposit deposit, Agent owner) : base(owner)
        {
            ref_deposit = new Reference<Deposit>(deposit);
            GetDeposit().SetPassbook(this);
        }

        [JsonConstructor]
        public Passbook(Reference<Deposit> ref_deposit, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.ref_deposit = ref_deposit;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            ref_deposit.SetReference(builder);
        }

        public Deposit GetDeposit()
        {
            return ref_deposit.Get();
        }

        public double GetBalance()
        {
            return GetDeposit().balance;
        }

        public double Withdraw(double amount)
        {
            return GetDeposit().Pay(amount);
        }

        public double Deposit(double amount)
        {
            return GetDeposit().Receive(amount);
        }

        public override double GetAssets()
        {
            return GetBalance();
        }

        public override double GetFinalAssetValue()
        {
            return GetBalance();
        }
    }
}
