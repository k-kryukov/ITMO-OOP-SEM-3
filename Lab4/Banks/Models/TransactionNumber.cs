using System.Text;
using Banks.Exceptions;

namespace Banks.Models;

public class TransactionNumber
{
    private string _number;
    private uint _lenOfNumber = 32;

    public TransactionNumber()
    {
        Random randomGenerator = new Random();

        var builder = new StringBuilder();
        while (builder.Length < _lenOfNumber)
            builder.Append(randomGenerator.Next(10).ToString());

        _number = builder.ToString();
    }

    public TransactionNumber(string s)
    {
        if (s.Length == _lenOfNumber)
            _number = s;
        else
            throw new InvalidTransactionNumber(s, _lenOfNumber);
    }

    public override string ToString() { return _number; }
}