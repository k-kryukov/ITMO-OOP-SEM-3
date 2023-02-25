using Banks.Accounts;
using Banks.Events;
using Banks.Exceptions;
using Banks.Models;
namespace Banks.Entities;

public class Client : IClient
{
    private string _name;
    private List<IAccount> _accounts;
    private IBank _bank;
    private PassportData? _passport = null;
    private string? _address = null;

    public Client(string name, IBank bank)
    {
        _name = name;
        _accounts = new List<IAccount>();
        _bank = bank;
    }

    public string Name { get { return _name; } }
    public IList<IAccount> Accounts { get { return _accounts; } }
    public bool IsSuspicious { get { return _passport == null || _address == null; } }

    public AccountNumber CreateDebitAccount(decimal percents)
    {
        var newAccount = new DebitAccount(percents, this);
        _accounts.Add(newAccount);
        return newAccount.Number;
    }

    public AccountNumber CreateCreditAccount(decimal percents, decimal maximumNegativeBalance)
    {
        var newAccount = new CreditAccount(percents, this, maximumNegativeBalance);
        _accounts.Add(newAccount);
        return newAccount.Number;
    }

    public AccountNumber CreateDepositAccount(decimal balance, SortedDictionary<decimal, decimal> depositPercentsMapping, decimal rateForHugeSum)
    {
        var newAccount = new DepositAccount(this, balance, depositPercentsMapping, rateForHugeSum);
        _accounts.Add(newAccount);
        return newAccount.Number;
    }

    public IAccount GetAccountByNumber(AccountNumber accountNumber)
    {
        IAccount? desiredAccount = (from account in _accounts
                        where account.Number.ToString() == accountNumber.ToString()
                        select account).FirstOrDefault();
        if (desiredAccount == null)
            throw new AccountDoesntExist(accountNumber);
        return desiredAccount;
    }

    public void SpecifyPersonalData(PassportData passport, string address)
    {
        _passport = passport;
        _address = address;
    }

    public void HandleEvent(IEvent strategy)
    {
        strategy.LogUpdate();
    }
}