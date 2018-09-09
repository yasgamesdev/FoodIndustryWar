using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class BankResources : AgentResources
    {
        public BankResources(Agent owner) : base(owner)
        {

        }

        [JsonConstructor]
        public BankResources(Reference<Agent> ref_owner, Dictionary<ResourceType, List<Resource>> resource_dict, Reference<FIS> ref_fis, int id) : base(ref_owner, resource_dict, ref_fis, id)
        {

        }

        public override double Pay(double amount)
        {
            if (amount > GetM1())
            {
                double shortage = amount - GetM1();
                Receive(GetBank().PrintMoney(shortage));
            }

            return GetCash().Pay(amount);
        }

        public override double Receive(double amount)
        {
            return GetCash().Receive(amount);
        }

        Cash GetCash()
        {
            return (Cash)GetResources(ResourceType.Cash)[0];
        }

        public double GetInterestRate(double principal)
        {
            double total_capital_demand = GetResources(ResourceType.Loan).ConvertAll(x => (Loan)x).Sum(x => x.principal) + principal;
            double total_capital_supply = GetHumans().GetM1() + GetCompanies().GetM1() + principal;

            return total_capital_supply != 0 ? (Math.Log(1.0 - (total_capital_demand / total_capital_supply)) / Math.Log(MEnv.human_cash_rate_base)) * 0.01 : 0;
        }

        //public double GetIndividualInterestRate(double principal, Agent owner)
        //{
        //    double remain_debt_amount = (owner.GetDebts().Sum(x => x.GetTotalPayAmount()) + principal) - owner.GetM1();
        //    if (remain_debt_amount <= 0)
        //    {
        //        return 0.0;
        //    }
        //    else
        //    {
        //        double days = remain_debt_amount / owner.GetTotalDailyNetIncome();
        //        if(days >= MEnv.loan_base_term * MEnv.individual_loan_rate)
        //        {
        //            return 1.0 * MEnv.individual_loan_rate;
        //        }
        //        else
        //        {
        //            return days / MEnv.loan_base_term;
        //        }
        //    }
        //}

        public double GetIndividualInterestRate(double principal, Agent owner)
        {
            double remain_debt_amount = (owner.GetDebts().Sum(x => x.GetTotalPayAmount()) + principal) - owner.GetM1();
            if (remain_debt_amount <= 0)
            {
                return 0.0;
            }
            else
            {
                double days = remain_debt_amount / owner.GetTotalDailyNetIncome();
                if (days >= MEnv.loan_base_term)
                {
                    return 0.1;
                }
                else
                {
                    return 0.1 * days / MEnv.loan_base_term;
                }
            }
        }

        public void BorrowMoney(Agent client, double principal)
        {
            //Loan loan = new Loan(principal, GetInterestRate(principal), GetOwner());
            Loan loan = new Loan(principal, GetInterestRate(principal) + GetIndividualInterestRate(principal, client), GetOwner());
            Debt debt = new Debt(loan, client);

            AddResource(ResourceType.Loan, loan);
            client.AddResource(ResourceType.Debt, debt);

            Deposit deposit = GetResources(ResourceType.Deposit).ConvertAll(x => (Deposit)x).First(x => x.GetPassbook().GetOwner() == client);
            deposit.Manipulate(principal);
            if(client is Company && ((Company)client).is_player)
            {
                client.Receive(0);
            }
        }

        //public double GetSupplyMoneyAmount()
        //{
        //    double cash = GetM0();
        //    double balances = GetResources(ResourceType.Deposit).ConvertAll(x => (Deposit)x).Sum(x => x.balance);
        //    double free_money = cash - balances;
        //    return free_money > MEnv.init_bank_money ? free_money - MEnv.init_bank_money : 0;
        //}

        public double GetSupplyMoneyAmount()
        {
            double cash = GetM0();
            double balances = GetResources(ResourceType.Deposit).ConvertAll(x => (Deposit)x).Sum(x => x.balance);
            double loans = GetResources(ResourceType.Loan).ConvertAll(x => (Loan)x).Sum(x => x.principal);
            double free_money = cash + loans - balances;
            return free_money > MEnv.init_bank_money ? free_money - MEnv.init_bank_money : 0;
        }
    }
}
