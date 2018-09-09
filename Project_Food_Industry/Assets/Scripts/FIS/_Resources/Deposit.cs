using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Deposit : Resource
    {
        [JsonProperty] Reference<Passbook> ref_book;
        public double balance { get; private set; }
        public double interest { get; private set; }

        public Deposit(Agent owner) : base(owner)
        {
            balance = 0;
            interest = 0;
        }

        public void SetPassbook(Passbook book)
        {
            ref_book = new Reference<Passbook>(book);
        }

        [JsonConstructor]
        public Deposit(Reference<Passbook> ref_book, double balance, double interest, Reference<Agent> ref_owner, Reference<FIS> ref_fis, int id) : base(ref_owner, ref_fis, id)
        {
            this.ref_book = ref_book;
            this.balance = balance;
            this.interest = interest;
        }

        public override void SetReferences(ReferenceBuilder builder)
        {
            base.SetReferences(builder);
            ref_book.SetReference(builder);
        }

        public Passbook GetPassbook()
        {
            return ref_book.Get();
        }

        public double Pay(double amount)
        {
            balance -= amount;
            if (balance < 0)
            {
                balance = 0;
            }
            return GetOwner().Pay(amount);
        }

        public double Receive(double amount)
        {
            balance += amount;
            return GetOwner().Receive(amount);
        }

        public double Manipulate(double amount)
        {
            balance += amount;
            if (balance < 0)
            {
                balance = 0;
            }
            return amount;
        }

        public override double GetLiabilities()
        {
            return balance;
        }

        public override double GetMaxInterestBearingDebt()
        {
            return balance;
        }

        public override double GetFinalAssetValue()
        {
            return -balance;
        }
    }
}
