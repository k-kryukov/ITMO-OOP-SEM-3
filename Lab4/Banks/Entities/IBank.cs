using Banks.Accounts;
using Banks.Events;
using Banks.Models;
namespace Banks.Entities;

public interface IBank
{
    string Name { get; }
    decimal CreditRate { get; }
    decimal DebitRate { get; }
    SortedDictionary<decimal, decimal> DepositPercentsMapping { get; }
    void ChangeCreditRate(decimal newRate);
    IClient GetClientByName(string clientName);
    void AddClient(string clientName);
    void SubscribeClient(string clientName, IEvent strategy);
    AccountNumber CreateDebitAccount(string clientName);
    AccountNumber CreateCreditAccount(string clientName);
    AccountNumber CreateDepositAccount(string clientName, decimal balance);
    void DisplayInfo();
    IAccount GetAccountByNumber(AccountNumber number);
    void ProcessPercents();
    TransactionNumber TopUpAccount(string clientName, AccountNumber accountNumber, decimal addedSum);
    TransactionNumber GetCash(string clientName, AccountNumber accountNumber, decimal sum);
    TransactionNumber TransferBetweenAccounts(string srcClientName, AccountNumber srcAccountHash, string dstClientName, AccountNumber dstAccountHash, decimal sum);
    void SpecifyClientPersonalData(string clientName, PassportData passport, string address);
    string ToString();

    void CancelSrcPay(AccountNumber number, decimal sum);
    void CancelDstPay(AccountNumber number, decimal sum);
}