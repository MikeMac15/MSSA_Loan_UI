using System;
using Spectre.Console.Cli;
using MortgageLibrary;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

namespace MSSA_Loan_1_CMDLN
{
    internal class LoanCLI : Command<LoanCLI.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [CommandOption("-a|--amount <AMOUNT>")]
            public double LoanAmount { get; set; }

            [CommandOption("-i|--interest <INTEREST>")]
            public double InterestRate { get; set; }

            [CommandOption("-t|--term <TERM>")]
            public int LoanTerm { get; set; }

            [CommandOption("--simulate")]
            public bool Simulate { get; set; }

            [CommandOption("--current")]
            public bool Current { get; set; }

            [CommandOption("-M|--currentmonth <MONTH>")]
            public int Month { get; set; }
            
            [CommandOption("-I|--interestpaid <PAID>")]
            public double InterestPaid { get; set; }
            
            [CommandOption("-P|--principalpaid <PPAID>")]
            public double PrincipalPaid { get; set; }

            [CommandOption("--help")]
            public bool Help { get; set; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            if (settings.Help)
            {
                AnsiConsole.MarkupLine("[bold green]Loan CLI Application Help[/]");
                AnsiConsole.MarkupLine("Usage:");
                AnsiConsole.WriteLine("Loan Calculator");
                AnsiConsole.WriteLine();
               // AnsiConsole.MarkupLine("[bold]Commands:[/]");
               // AnsiConsole.MarkupLine("  [bold]loan[/]     Manage your loans");
               // AnsiConsole.MarkupLine("  [bold]help[/]     Display help information");
               // AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[bold]Options for Loan Calculator:[/]");
                AnsiConsole.MarkupLine("  -a|--amount <AMOUNT>       The loan amount.");
                AnsiConsole.MarkupLine("  -i|--interest <INTEREST>   The interest rate (in %).");
                AnsiConsole.MarkupLine("  -t|--term <TERM>           The loan term (in months).");
                AnsiConsole.MarkupLine("  --simulate                  Simulate loan payments.");
                AnsiConsole.MarkupLine("  --current                   Update the current loan state.");
                AnsiConsole.MarkupLine("  -M|--currentmonth <MONTH>  The current month.");
                AnsiConsole.MarkupLine("  -I|--interestpaid <PAID>   Total interest paid.");
                AnsiConsole.MarkupLine("  -P|--principalpaid <PPAID> Total principal paid.");
                return 0;
            }
                

            if (settings.LoanAmount <= 0 || settings.InterestRate <= 0 || settings.LoanTerm <= 0)
            {
                AnsiConsole.MarkupLine("[red]Invalid loan parameters. Please ensure all values are positive.[/]");
                return 0;
            }

            Loan loan = new Loan(settings.LoanAmount, settings.InterestRate, settings.LoanTerm);

            if (settings.Current)
            {
                if (settings.Month < 0 || settings.InterestPaid < 0 || settings.PrincipalPaid < 0)
                {
                    AnsiConsole.MarkupLine("[red]Invalid current loan state parameters. Please ensure all values are positive.[/]");
                    return 0;
                }
                loan.SetCurrentLoanState(settings.Month, settings.InterestPaid, settings.PrincipalPaid);
            }

            if (settings.Simulate)
            {
                List<LoanMonthDetails> simLoanList = loan.SimulateLoan();
                LoanSimTable loanSimTable = new LoanSimTable(simLoanList);
                loanSimTable.Render();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No operation specified. Use --simulate or --current to perform actions.[/]");
                return 0;
            }

            return 0;
        }
    }
}