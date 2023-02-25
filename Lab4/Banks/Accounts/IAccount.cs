using Banks.Entities;
using Banks.Models;
namespace Banks.Accounts;

public abstract class IAccount
{
    protected decimal _balance = 0;
    protected decimal _percents = 0;
    protected AccountNumber _number = null!;
    protected decimal _buffer = 0;
    protected uint timeAfterLastBufferRelease = 0;
    protected uint dayInMonth = 30;

    public abstract string ClientName { get; }
    public decimal Balance { get { return _balance; } }
    public abstract string AccountType { get; }
    public decimal Percents { get { return _percents; } set { _percents = value; } }
    public AccountNumber Number { get { return _number; } }
    public abstract bool IsClientSuspicious { get; }

    public abstract bool IsAbleToTransfer(decimal sum);
    public virtual void TopUpAccount(decimal addedSum) { _balance += addedSum; }
    public abstract void WithdrawMoney(decimal sum, bool force = false);
    public abstract void ProcessPercents();

    public override string ToString()
    {
        return $"Client {ClientName} (suspicious: {IsClientSuspicious}) has {Balance} on {AccountType} account {Number} with {Percents}% bank rate";
    }
}