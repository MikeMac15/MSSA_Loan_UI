using System;
using System.Collections.Generic;
using Spectre.Console;
using MortgageLibrary;
using Spectre.Console.Cli;

namespace MSSA_Loan_1_CMDLN
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Check if there are command-line arguments provided
            if (args.Length > 0)
            {
                // Run the CLI mode
                var app = new CommandApp<LoanCLI>();
                app.Run(args);
            }
            else
            {
                // Run the interactive mode
                RunInteractiveMode();
            }
        }

        public static void DisplayLoanDetails(Loan loan)
        {
            //AnsiConsole.MarkupLine($"~Loan Agreement~");
            //AnsiConsole.MarkupLine($"Loan Amount: ${loan.originalLoanAmount}  Interest Rate: {loan.interestRate}%  Term in months: {loan.loanTerm}");
            //AnsiConsole.MarkupLine($"-Current Loan Status-");
            //AnsiConsole.MarkupLine($"Remaining Balance: ${loan.remainingLoanAmount}  Interest Paid: ${loan.totalInterestPaid}  Principal Paid: ${loan.totalPrincipalPaid}");


            string originalLoanAmount = loan.originalLoanAmount.ToString("C");
            double interestRate = loan.interestRate;
            int loanTerm = loan.loanTerm;
            string remainingBalance = loan.remainingLoanAmount.ToString("C");
            string interestPaid = loan.totalInterestPaid.ToString("C");
            string principalPaid = loan.totalPrincipalPaid.ToString("C");


            AnsiConsole.Write(
                new Rule("[bold yellow]~ Loan Agreement ~[/]")
            );
            AnsiConsole.WriteLine();

            AnsiConsole.Write(
                new Table()
                .Centered()
                .HideHeaders()
                .AddColumn(new TableColumn("").Centered())
                .AddColumn(new TableColumn("").Centered())
                .AddRow("Loan Amount", $"{originalLoanAmount}")
                .AddRow("Interest Rate", $"{interestRate}%")
                .AddRow("Term in Months", loanTerm.ToString())
                .AddRow("Monthly Payment", loan.minMonthlyPayment.ToString("C"))

            );

            AnsiConsole.WriteLine();
            AnsiConsole.Write(
                new Rule("[bold yellow]~ Current Loan Status ~[/]")
            );

            AnsiConsole.Write(
                new Table()
                .Centered()
                .HideHeaders()
                .AddColumn(new TableColumn("").Centered())
                .AddColumn(new TableColumn("").Centered())
                .AddRow("Remaining Balance", $"{remainingBalance}")
                .AddRow("Interest Paid", $"{interestPaid}")
                .AddRow("Principal Paid", $"{principalPaid}")
            );




        }

        private static void ExecuteChoice(string choice, Loan loan) {
            switch (choice)
            {
                case "Loan Details":
                    DisplayLoanDetails(loan);
                    AnsiConsole.WriteLine();
                    break;
                case "Make Payment":
                    double payment = AnsiConsole.Ask<double>("[bold green]Enter payment amount: [/]");
                    AnsiConsole.WriteLine();
                    loan.MakePayment(payment);
                    AnsiConsole.MarkupLine($"[bold green]Thank you for your payment of ${payment}[/]");
                    AnsiConsole.WriteLine();
                    break;
                case "Months Left On Loan":
                    int monthsLeft = loan.CalculateRemainingMonthsForMinPayment();
                    AnsiConsole.MarkupLine($"You have {monthsLeft} months left on your loan.");
                    AnsiConsole.WriteLine();
                    break;
                case "Update Loan State":
                    var month = AnsiConsole.Ask<int>("Enter the current month:");
                    var interestPaid = AnsiConsole.Ask<double>("Enter the total interest paid:");
                    var principalPaid = AnsiConsole.Ask<double>("Enter the total principal paid:");
                    loan.SetCurrentLoanState(month, interestPaid, principalPaid);
                    AnsiConsole.MarkupLine($"[bold green]Loan state updated.[/]");
                    AnsiConsole.WriteLine();
                    break;
                case "Display Simulated Loan Table":
                    List<LoanMonthDetails> simLoanList = loan.SimulateLoan();
                    LoanSimTable loanSimTable = new LoanSimTable(simLoanList);
                    loanSimTable.Render();
                    AnsiConsole.WriteLine();
                    break;

                default:
                    AnsiConsole.MarkupLine("[grey]Invalid input.[/]");
                    AnsiConsole.WriteLine();
                    break;
            }
        }

        private static void RunInteractiveMode()
        {
            // Display a welcome message
            AnsiConsole.MarkupLine("[bold green]Welcome to the Loan Calculator![/]");
            // Read input values
            var loanAmount = AnsiConsole.Ask<double>("Enter the loan amount:");
            var interestRate = AnsiConsole.Ask<double>("Enter the interest rate percentage:");
            var loanTerm = AnsiConsole.Ask<int>("Enter the loan term (in months):");

            Loan loan = new Loan(loanAmount, interestRate, loanTerm);

            string cont = "Y";
            // Loop to allow the user to calculate multiple loans
            while (cont.ToUpper() == "Y")
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("How can we help?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(new[] {
                            "Loan Details", "Make Payment", "Months Left On Loan",
                            "Update Loan State", "Display Simulated Loan Table"
                        }));
                ExecuteChoice(choice, loan);


                cont = AnsiConsole.Ask<string>("Would you like to perform another operation? (Y/N)");
                AnsiConsole.WriteLine();
            }
            AnsiConsole.MarkupLine("Have a great day.");

        }
    }
}