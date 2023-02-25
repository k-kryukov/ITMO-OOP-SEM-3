namespace Banks.Models;

public class Transaction : ITransaction
{
    private AccountNumber? _srcAccountNumber;
    private AccountNumber? _dstAccountNumber;
    private string? _srcBankName;
    private string? _dstBankName;
    private TransactionNumber _id;
    private decimal _sum;

    public Transaction(
            AccountNumber? srcAccountNumber,
            AccountNumber? dstAccountNumber,
            decimal sum,
            string? srcBankName,
            string? dstBankName)
    {
        _srcAccountNumber = srcAccountNumber;
        _dstAccountNumber = dstAccountNumber;
        _sum = sum;
        _srcBankName = srcBankName;
        _dstBankName = dstBankName;
        _id = new TransactionNumber();
    }

    public TransactionNumber Id { get { return _id; } }
    public AccountNumber? SrcAccountNumber { get { return _srcAccountNumber; } }
    public AccountNumber? DstAccountNumber { get { return _dstAccountNumber; } }
    public string? SrcBankName { get { return _srcBankName; } }
    public string? DstBankName { get { return _dstBankName; } }
    public decimal Sum { get { return _sum; } }

    public override string ToString()
    {
        string defaultString = "-";
        string? srcAccountString, dstAccountString;
        if (_srcAccountNumber == null)
            srcAccountString = defaultString;
        else
            srcAccountString = _srcAccountNumber.ToString();
        if (_dstAccountNumber == null)
            dstAccountString = defaultString;
        else
            dstAccountString = _dstAccountNumber.ToString();
        return $"Transaction {_id} from {srcAccountString} to {dstAccountString} with sum {_sum}";
    }
}