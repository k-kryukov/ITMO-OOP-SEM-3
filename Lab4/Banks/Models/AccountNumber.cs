using System.Text;
using Banks.Exceptions;

namespace Banks.Models;

public class AccountNumber
{
    private string _number;
    private uint _lenOfNumber = 16;

    public AccountNumber()
    {
        Random randomGenerator = new Random();

        var builder = new StringBuilder();
        while (builder.Length < _lenOfNumber)
            builder.Append(randomGenerator.Next(10).ToString());

        _number = builder.ToString();
    }

    public AccountNumber(string s)
    {
        if (s.Length == _lenOfNumber)
            _number = s;
        else
            throw new InvalidAccountNumber(s, _lenOfNumber);
    }

    public override string ToString() { return _number; }
}