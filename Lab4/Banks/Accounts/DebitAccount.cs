using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;
namespace Banks.Accounts;

public class DebitAccount : IAccount
{
    private IClient _client;

    public DebitAccount(decimal percents, IClient client)
    {
        _balance = 0;
        _percents = percents;
        _client = client;
        _number = new AccountNumber();
    }

    public override string ClientName { get { return _client.Name; } }
    public override bool IsClientSuspicious { get { return _client.IsSuspicious; } }
    public override string AccountType { get { return "Debit"; } }

    public override void WithdrawMoney(decimal sum, bool force = false)
    {
        if (_balance < sum && !force)
            throw new NotEnoughMoneyOnAccount(_number, _balance, sum);
        _balance -= sum;
    }

    public override bool IsAbleToTransfer(decimal sum)
    {
        return _balance - sum >= 0;
    }

    public override void ProcessPercents()
    {
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