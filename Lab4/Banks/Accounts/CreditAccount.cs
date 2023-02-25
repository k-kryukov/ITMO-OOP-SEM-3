using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;
namespace Banks.Accounts;

public class CreditAccount : IAccount
{
    private IClient _client;
    private decimal _maximumNegativeBalance;

    public CreditAccount(decimal percents, IClient client, decimal maximumNegativeBalance)
    {
        _balance = 0;
        _percents = percents;
        _client = client;
        _maximumNegativeBalance = maximumNegativeBalance;
        _number = new AccountNumber();
    }

    public override string ClientName { get { return _client.Name; } }
    public override bool IsClientSuspicious { get { return _client.IsSuspicious; } }
    public override string AccountType { get { return "Credit"; } }

    public override void WithdrawMoney(decimal sum, bool force = false)
    {
        if (_balance - sum >= -_maximumNegativeBalance && !force)
            _balance -= sum;
        else
            throw new CreditLimitIsReached(sum, _maximumNegativeBalance, _number);
    }

    public override bool IsAbleToTransfer(decimal sum)
    {
        return _balance - sum >= -_maximumNegativeBalance;
    }

    public override void ProcessPercents()
    {
        if (_balance >= 0)
            return;

        _buffer += _balance * (_percents / 100 / 365);
        timeAfterLastBufferRelease++;
        if (timeAfterLastBufferRelease == dayInMonth)
        {
            _balance += _buffer;
            _buffer = 0;
            timeAfterLastBufferRelease = 0;
        }
    }
}