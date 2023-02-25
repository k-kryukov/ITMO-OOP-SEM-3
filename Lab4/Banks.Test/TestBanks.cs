using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void TestAddBank_BankIsCreated()
    {
        string testBankName = "Sperbank";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 5, depositPercents, 1000);
        IBank bank = centralBank.GetBankByName(testBankName);
    }

    [Fact]
    public void TestAddClient_ClientIsCreated()
    {
        string testBankName = "Raiffeisen";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 5, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        IClient client = bank.GetClientByName("k.kryukov");
    }

    [Fact]
    public void TestAddAccounts_AccountsAreCreated()
    {
        string testBankName = "VTB";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 5, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        bank.SpecifyClientPersonalData("k.kryukov", new PassportData("4016", "639090"), "Pushkina 1");
        IClient client = bank.GetClientByName("k.kryukov");
        bank.CreateDebitAccount("k.kryukov");
        bank.CreateCreditAccount("k.kryukov");
        bank.CreateDepositAccount("k.kryukov", 2000);
    }

    [Fact]
    public void TestSuspiciousAccount()
    {
        string testBankName = "Otkritie";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 5, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        var debit = bank.CreateDebitAccount("k.kryukov");
        Assert.Throws<CreditAccountNotAllowedForSuspiciousClient>(() => bank.CreateCreditAccount("k.kryukov"));
        Assert.Throws<TooManyMoneyOnSuspiciousAccount>(() => bank.TopUpAccount("k.kryukov", debit, 100000));

        bank.SpecifyClientPersonalData("k.kryukov", new PassportData("4016", "639090"), "Pushkina 1");
        bank.CreateDepositAccount("k.kryukov", 20000);
        bank.CreateCreditAccount("k.kryukov");
        bank.TopUpAccount("k.kryukov", debit, 10000);
    }

    [Fact]
    public void DepositAccountUnavailableBeforeTimeout()
    {
        string testBankName = "Pochta Bank";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 5, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        var deposit = bank.CreateDepositAccount("k.kryukov", 20000);

        Assert.Throws<DepositMoneyWithdrawUnavailable>(() => bank.GetCash("k.kryukov", deposit, 2000));
        Time.MoveTimeForward(500);
        bank.GetCash("k.kryukov", deposit, 2000);
    }

    [Fact]
    public void TopUpAccount_MoneyComes()
    {
        string testBankName = "Sber";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 5, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        var debit = bank.CreateDebitAccount("k.kryukov");
        bank.TopUpAccount("k.kryukov", debit, 200);
        Assert.True(bank.GetAccountByNumber(debit).Balance == 200);
    }

    [Fact]
    public void AfterWhile_PercentsCalculated()
    {
        string testBankName = "Tinkoff";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 5, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        bank.SpecifyClientPersonalData("k.kryukov", new PassportData("4016", "639090"), "Pushkina 1");
        var debit = bank.CreateDebitAccount("k.kryukov");
        bank.TopUpAccount("k.kryukov", debit, 200);
        var credit = bank.CreateCreditAccount("k.kryukov");
        bank.GetCash("k.kryukov", credit, 500);

        Time.MoveTimeForward(390);

        Assert.True(bank.GetAccountByNumber(debit).Balance > (decimal)(200 * 1.05));
        Assert.True(bank.GetAccountByNumber(credit).Balance < (decimal)(-500 * 1.05));
    }

    [Fact]
    public void CreditLimitReached_ExceptionHappens()
    {
        string testBankName = "Gazprom";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 10, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        bank.SpecifyClientPersonalData("k.kryukov", new PassportData("4016", "639090"), "Pushkina 1");
        var credit = bank.CreateCreditAccount("k.kryukov");
        bank.GetCash("k.kryukov", credit, 800);
        Assert.Throws<CreditLimitIsReached>(() => bank.GetCash("k.kryukov", credit, 300));
        Time.MoveTimeForward(1000);
        Assert.Throws<CreditLimitIsReached>(() => bank.GetCash("k.kryukov", credit, 1));
    }

    [Fact]
    public void TransferIsSuccessful()
    {
        string testBankName = "SPB";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 10, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        bank.SpecifyClientPersonalData("k.kryukov", new PassportData("4016", "639090"), "Pushkina 1");
        var credit = bank.CreateCreditAccount("k.kryukov");
        var debit = bank.CreateDebitAccount("k.kryukov");

        bank.TopUpAccount("k.kryukov", credit, 1000);
        bank.TopUpAccount("k.kryukov", debit, 1000);

        bank.TransferBetweenAccounts("k.kryukov", credit, "k.kryukov", debit, 500);
        Assert.True(bank.GetAccountByNumber(credit).Balance == 500 && bank.GetAccountByNumber(debit).Balance == 1500);
    }

    [Fact]
    public void TransactionIsCancelled_MoneyReturnsBack()
    {
        string testBankName = "PSB";
        CentralBank centralBank = CentralBank.GetInstance();
        SortedDictionary<decimal, decimal> depositPercents = new SortedDictionary<decimal, decimal>();
        depositPercents.Add(200, 2);
        depositPercents.Add(4000, 3);
        depositPercents.Add(10000, 5);

        centralBank.AddBank(testBankName, 5, 10, depositPercents, 1000);
        IBank bank = CentralBank.GetInstance().GetBankByName(testBankName);
        bank.AddClient("k.kryukov");
        bank.SpecifyClientPersonalData("k.kryukov", new PassportData("4016", "639090"), "Pushkina 1");
        var credit = bank.CreateCreditAccount("k.kryukov");
        var debit = bank.CreateDebitAccount("k.kryukov");

        bank.TopUpAccount("k.kryukov", credit, 1000);
        bank.TopUpAccount("k.kryukov", debit, 1000);

        var trans = bank.TransferBetweenAccounts("k.kryukov", credit, "k.kryukov", debit, 500);
        centralBank.CancelTransaction(trans);
        Assert.True(bank.GetAccountByNumber(credit).Balance == 1000 && bank.GetAccountByNumber(debit).Balance == 1000);
    }
}