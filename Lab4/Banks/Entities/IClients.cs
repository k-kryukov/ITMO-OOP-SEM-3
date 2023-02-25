using Banks.Accounts;
using Banks.Events;
using Banks.Models;
namespace Banks.Entities;

public interface IClient
{
    string Name { get; }
    IList<IAccount> Accounts { get; }
    bool IsSuspicious { get; }
    AccountNumber CreateDebitAccount(decimal percents);
    AccountNumber CreateCreditAccount(decimal percents, decimal maximumNegativeBalance);
    AccountNumber CreateDepositAccount(decimal balance, SortedDictionary<decimal, decimal> depositPercentsMapping, decimal rateForHugeSum);
    IAccount GetAccountByNumber(AccountNumber accountNumber);
    void SpecifyPersonalData(PassportData passport, string address);
    void HandleEvent(IEvent strategy);
}