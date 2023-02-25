using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;
namespace Program;

public class BankInterface
{
    private IBank _bank;

    public BankInterface(IBank bank)
    {
        _bank = bank;
    }

    public void PrintHint()
    {
        Console.WriteLine("-1 - Go back to CB menu, 0 - create client, 1 - create account, 2 - top up account, 3 - get cash from account\n" +
                          "4 - display this bank info, 5 - transfer between accounts");
    }

    public void CreateClient()
    {
        Console.WriteLine("Input client name!");
        string name = Console.ReadLine() ?? "-1";
        try
        {
            _bank.AddClient(name);
            Console.WriteLine($"Created client {name}!");
        }
        catch (ClientAlreadyExists e)
        {
            Console.WriteLine(e.Message);
            if (!_bank.GetClientByName(name).IsSuspicious)
                return;
        }

        Console.WriteLine("Do you want specify client additional data in order to get full access to bank?\nType y/yes to continue");
        string answer = Console.ReadLine() ?? "-1";
        if (answer != "y" && answer != "yes")
            return;

        Console.WriteLine("Specify passport series");
        string passportSeries = Console.ReadLine() ?? "-1";
        Console.WriteLine("Specify passport number");
        string passportNumber = Console.ReadLine() ?? "-1";
        var passport = new PassportData(passportSeries, passportNumber);

        Console.WriteLine("Specify home address");
        string address = Console.ReadLine() ?? "-1";

        _bank.SpecifyClientPersonalData(name, passport, address);
    }

    public void CreateAccount()
    {
        Console.WriteLine("Specify client (account owner)");
        string clientName = Console.ReadLine() ?? "-1";
        Console.WriteLine("Input account type (debit(Debit) | deposit(Deposit) | credit(Credit))");
        string type = Console.ReadLine() ?? "-1";
        if (type == "debit" || type == "Debit")
        {
            _bank.CreateDebitAccount(clientName);
        }
        else if (type == "credit" || type == "Credit")
        {
            try
            {
                _bank.CreateCreditAccount(clientName);
            }
            catch (CreditAccountNotAllowedForSuspiciousClient e)
            {
                Console.WriteLine(e.Message);
            }
        }
        else if (type == "deposit" || type == "Deposit")
        {
            Console.WriteLine("Specify initial balance (rate depends on it)");
            var initialBalance = Convert.ToDecimal(Console.ReadLine());
            _bank.CreateDepositAccount(clientName, initialBalance);
        }
        else
        {
            Console.WriteLine("Wrong account type!");
        }
    }

    public void TopUpAccount()
    {
        Console.WriteLine("Specify client (account owner)");
        string clientName = Console.ReadLine() ?? "-1";
        Console.WriteLine("Specify replenishment sum");
        var sum = Convert.ToDecimal(Console.ReadLine());
        Console.WriteLine("Specify account number (16-digit number)");
        string num = Console.ReadLine() ?? "-1";

        AccountNumber accountNumber;
        try
        {
            accountNumber = new AccountNumber(num);
        }
        catch (InvalidAccountNumber e)
        {
            Console.WriteLine(e.Message);
            return;
        }

        try
        {
            _bank.TopUpAccount(clientName, accountNumber, sum);
        }
        catch (Exception e) when (e is TooManyMoneyOnSuspiciousAccount ||
                                  e is ClientDoesntExist ||
                                  e is AccountDoesntExist)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void GetCash()
    {
        Console.WriteLine("Specify client (account owner)");
        string clientName = Console.ReadLine() ?? "-1";
        Console.WriteLine("Specify wanted sum");
        var sum = Convert.ToDecimal(Console.ReadLine());
        Console.WriteLine("Specify account number (16-digit number)");
        string num = Console.ReadLine() ?? "-1";

        AccountNumber accountNumber;
        try
        {
            accountNumber = new AccountNumber(num);
        }
        catch (InvalidAccountNumber e)
        {
            Console.WriteLine(e.Message);
            return;
        }

        try
        {
            _bank.GetCash(clientName, accountNumber, sum);
        }
        catch (Exception e) when (e is CreditLimitIsReached ||
                                  e is NotEnoughMoneyOnAccount ||
                                  e is ClientDoesntExist ||
                                  e is AccountDoesntExist)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void DisplayInfo()
    {
        _bank.DisplayInfo();
    }

    public void TransferBetweenAccounts()
    {
        Console.WriteLine("Input source client name");
        string srcClientName = Console.ReadLine() ?? "-1";
        Console.WriteLine("Input destination client name");
        string dstClientName = Console.ReadLine() ?? "-1";
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
            return;
        }

        try
        {
            _bank.TransferBetweenAccounts(srcClientName, srcAccountNumber, dstClientName, dstAccountNumber, sum);
        }
        catch (Exception e) when (e is BankDoesntExist ||
                        e is AccountDoesntExist ||
                        e is UnableToTransferBetweenAccounts)
        {
            Console.WriteLine(e.Message);
        }
    }
}