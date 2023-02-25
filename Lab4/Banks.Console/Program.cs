using System;
using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;
namespace Program;

public class Program
{
    public static void Main(string[] args)
    {
        var centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank("Sber", 2, 2, depositPercents, 500);
        var sber = centralBank.GetBankByName("Sber");
        sber.AddClient("k.kryukov");
        var debit = sber.CreateDebitAccount("k.kryukov");
        sber.TopUpAccount("k.kryukov", debit, 200);

        while (true)
        {
            PrintGeneralHint();

            int opt = Convert.ToInt32(Console.ReadLine());
            if (opt == -1)
            {
                return;
            }
            else if (opt == 0)
            {
                Console.WriteLine("Input new bank name!");
                string name = Console.ReadLine() ?? "-1";
                Console.WriteLine("Add a rate for debit account!");
                decimal debitRate = Convert.ToDecimal(Console.ReadLine() ?? "-1");
                Console.WriteLine("Add a rate for credit account!");
                decimal creditRate = Convert.ToDecimal(Console.ReadLine() ?? "-1");
                Console.WriteLine("Specify maximum negative balance for credit account!");
                decimal maxNegativeCreditBalance = Convert.ToDecimal(Console.ReadLine() ?? "-1");
                SortedDictionary<decimal, decimal> mapping = new SortedDictionary<decimal, decimal>();
                Console.WriteLine("Describe deposit rate schema: input number of borders and rate for sum less or equal, than right border");
                int n = Convert.ToInt32(Console.ReadLine() ?? "-1");
                for (int i = 0; i < n; ++i)
                {
                    Console.WriteLine("Input a border (e.g. less or equal than 200)");
                    decimal border = Convert.ToDecimal(Console.ReadLine());
                    Console.WriteLine("Specify a percents for sums which are less or equal than this border");
                    decimal rate = Convert.ToDecimal(Console.ReadLine());
                    mapping.Add(border, rate);
                }

                centralBank.AddBank(name, debitRate, creditRate, mapping, maxNegativeCreditBalance);
                Console.WriteLine("Bank successfully created!");
            }
            else if (opt == 1)
            {
                centralBank.DisplayInfo();
            }
            else if (opt == 2)
            {
                Console.WriteLine("Input desired bank name!");
                string name = Console.ReadLine() ?? "-1";
                IBank bank;
                try
                {
                    bank = centralBank.GetBankByName(name);
                }
                catch (BankDoesntExist e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                var bankInterface = new BankInterface(bank);
                while (true)
                {
                    bankInterface.PrintHint();
                    opt = Convert.ToInt32(Console.ReadLine());
                    if (opt == -1)
                    {
                        break;
                    }
                    else if (opt == 0)
                    {
                        bankInterface.CreateClient();
                    }
                    else if (opt == 1)
                    {
                        try
                        {
                            bankInterface.CreateAccount();
                        }
                        catch (ClientDoesntExist e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else if (opt == 2)
                    {
                        bankInterface.TopUpAccount();
                    }
                    else if (opt == 3)
                    {
                        bankInterface.GetCash();
                    }
                    else if (opt == 4)
                    {
                        bankInterface.DisplayInfo();
                    }
                    else if (opt == 5)
                    {
                        bankInterface.TransferBetweenAccounts();
                    }
                    else
                    {
                        Console.WriteLine("Wrong option!");
                    }
                }
            }
            else if (opt == 3)
            {
                Console.WriteLine("Input number of days to move time forward!");
                var daysAmount = Convert.ToUInt32(Console.ReadLine());
                Time.MoveTimeForward(daysAmount);
            }
            else if (opt == 4)
            {
                Console.WriteLine("Input source bank name");
                string srcBankName = Console.ReadLine() ?? "-1";
                Console.WriteLine("Input destination bank name");
                string dstBankName = Console.ReadLine() ?? "-1";
                Console.WriteLine("Input source account number");
                string srcNum = Console.ReadLine() ?? "-1";
                Console.WriteLine("Input destination account number");
                string dstNum = Console.ReadLine() ?? "-1";
                Console.WriteLine("Input a sum which you want to transfer");
                decimal sum = Convert.ToDecimal(Console.ReadLine());

                AccountNumber srcAccountNumber, dstAccountNumber;
                try
                {
                    srcAccountNumber = new AccountNumber(srcNum);
                    dstAccountNumber = new AccountNumber(dstNum);
                }
                catch (InvalidAccountNumber e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                try
                {
                    centralBank.TransferBetweenBanks(srcBankName, dstBankName, srcAccountNumber, dstAccountNumber, sum);
                }
                catch (Exception e) when (e is BankDoesntExist ||
                                  e is AccountDoesntExist ||
                                  e is UnableToTransferBetweenBanks)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (opt == 5)
            {
                foreach (var trans in centralBank.TransactionHistory)
                {
                    Console.WriteLine(trans);
                }
            }
            else if (opt == 6)
            {
                Console.WriteLine("Enter a transaction number which you want to cancel!");
                string num = Console.ReadLine() ?? "-1";
                TransactionNumber transactionNumber;
                try
                {
                    transactionNumber = new TransactionNumber(num);
                }
                catch (InvalidTransactionNumber e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                centralBank.CancelTransaction(transactionNumber);
                Console.WriteLine($"Cancelled transaction {transactionNumber}");
            }
            else
            {
                Console.WriteLine("Wrong option number!");
            }
        }
    }

    private static void PrintGeneralHint()
    {
        Console.WriteLine("-1 - exit, 0 - add bank, 1 - print all info, 2 - go to bank menu, 3 - move time forward, " +
                          "4 - transfer between banks\n5 - display all transactions, 6 - cancel transaction");
    }
}