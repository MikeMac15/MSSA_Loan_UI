using MortgageLibrary;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSA_Loan_1_CMDLN
{
    internal class LoanSimTable
    {
        public List<LoanMonthDetails> MonthlyDetailsList;
        public LoanSimTable(List<LoanMonthDetails> simLoan)
        {
            MonthlyDetailsList = simLoan;
        }

        public void Render()
        {

            double totalInterest = MonthlyDetailsList.Sum(x => x.InterestPayment);


            var table = new Table()
            .Centered()
            .AddColumn("Month")
            .AddColumn("Payment")
            .AddColumn("Principal")
            .AddColumn("Interest")
            .AddColumn("Balance");
            table.Title = new TableTitle("Loan Simulation Table");

            foreach (var month in MonthlyDetailsList)
            {
                table.AddRow(month.Month.ToString(), month.Payment.ToString("C"), month.PrincipalPayment.ToString("C"), month.InterestPayment.ToString("C"), month.RemainingBalance.ToString("C"));
            }
        

        
            AnsiConsole.Write(table);
        }
    }
}
