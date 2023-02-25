using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities;

public class CentralBank : ICentralBank
{
    private static CentralBank? _instance;
    private List<IBank> _banks;
    private List<ITransaction> _transactionHistory;

    private CentralBank()
    {
        _banks = new List<IBank>();
        _transactionHistory = new List<ITransaction>();
        Console.WriteLine("Central bank created");
    }

    public IList<ITransaction> TransactionHistory { get { return _transactionHistory; } }
    public IList<IBank> Banks { get { return _banks; } }

    public static CentralBank GetInstance()
    {
        if (_instance == null)
            _instance = new CentralBank();
        return _instance;
    }

    public IBank AddBank(
        string bankName,
        decimal debitPercents,
        decimal creditPercents,
        SortedDictionary<decimal, decimal> depositPercentsMapping,
        decimal maximumNegativeBalance)
    {
        IBank? existingBank = (from bank in _banks
                        where bank.Name == bankName
                        select bank).FirstOrDefault();
        if (existingBank != null)
            throw new BankAlreadyExists(bankName);

        var newBank = new Bank(bankName, debitPercents, creditPercents, depositPercentsMapping, maximumNegativeBalance);
        _banks.Add(newBank);
        return newBank;
    }

    public void SaveTransaction(ITransaction trans)
    {
        _transactionHistory.Add(trans);
    }

    public IBank GetBankByName(string bankName)
    {
        IBank? desiredBank = (from bank in _banks
                        where bank.Name == bankName
                        select bank).FirstOrDefault();
        if (desiredBank == null)
            throw new BankDoesntExist(bankName);
        return desiredBank;
    }

    public TransactionNumber TransferBetweenBanks(string srcBankName, string dstBankName, AccountNumber srcAccountHash, AccountNumber dstAccountHash, decimal sum)
    {
        var srcBank = GetBankByName(srcBankName);
        var dstBank = GetBankByName(dstBankName);

        var srcAccount = srcBank.GetAccountByNumber(srcAccountHash);
        var dstAccount = dstBank.GetAccountByNumber(dstAccountHash);

        if (srcAccount.IsAbleToTransfer(sum))
        {
            srcAccount.WithdrawMoney(sum);
            dstAccount.TopUpAccount(sum);
            ITransaction transaction = new Transaction(srcAccountHash, dstAccountHash, sum, srcBankName, dstBankName);
            SaveTransaction(transaction);
            return transaction.Id;
        }
        else
        {
            throw new UnableToTransferBetweenBanks(srcBankName, dstBankName, srcAccountHash, dstAccountHash, sum);
        }
    }

    public ITransaction GetTransactionByNumber(TransactionNumber number)
    {
        ITransaction desiredTransaction = (from transaction in _transactionHistory
                        where transaction.Id.ToString() == number.ToString()
                        select transaction).FirstOrDefault() ?? throw new TransactionDoesntExist(number);
        return desiredTransaction;
    }

    public void CancelTransaction(TransactionNumber number)
    {
        ITransaction transaction = GetTransactionByNumber(number);
        var srcBankName = transaction.SrcBankName;
        var dstBankName = transaction.DstBankName;

        // TopUpBalance
        if (srcBankName != null)
        {
            var srcBank = GetBankByName(srcBankName);
            srcBank.CancelSrcPay(transaction.SrcAccountNumber, transaction.Sum);
        }

        // GetCash
        if (dstBankName != null)
        {
            var dstBank = GetBankByName(dstBankName);
            dstBank.CancelDstPay(transaction.DstAccountNumber, transaction.Sum);
        }

        _transactionHistory.Remove(transaction);
    }

    public void NotifyTimeToCalculatePercents()
    {
        foreach (var bank in _banks)
            bank.ProcessPercents();
    }

    public void DisplayInfo()
    {
        Console.WriteLine("************* CENTRAL BANK INFORMATION *************");
        foreach (var bank in _banks)
        {
            Console.WriteLine($"Bank {bank}");
            bank.DisplayInfo();
        }

        Console.WriteLine("************* END OF CENTRAL BANK INFORMATION *************");
    }
}