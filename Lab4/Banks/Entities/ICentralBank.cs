using Banks.Models;
namespace Banks.Entities;

public interface ICentralBank
{
    TransactionNumber TransferBetweenBanks(string srcBankName, string dstBankName, AccountNumber srcAccountHash, AccountNumber dstAccountHash, decimal sum);
    IBank AddBank(
        string bankName,
        decimal debitPercents,
        decimal creditPercents,
        SortedDictionary<decimal, decimal> depositPercentsMapping,
        decimal maximumNegativeBalance);
    void NotifyTimeToCalculatePercents();
}