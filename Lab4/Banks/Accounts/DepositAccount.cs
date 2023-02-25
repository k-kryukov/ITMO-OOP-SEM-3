using Banks.Entities;
using Banks.Exceptions;
using Banks.Models;
namespace Banks.Accounts;

public class DepositAccount : IAccount
{
    private static uint _timeout = 365;
    private IClient _client;
    private DepositPercentsCalculator _calculator;
    private uint _createdTime;

    public DepositAccount(IClient client, decimal balance, SortedDictionary<decimal, decimal> percentsMapping, decimal rateForHugeSum)
    {
        _balance = balance;
        _calculator = new DepositPercentsCalculator(new SortedDictionary<decimal, decimal>(percentsMapping), rateForHugeSum);
        _percents = _calculator.CalculatePercents(balance);
        _client = client;
        _number = new AccountNumber();
        _createdTime = Time.CurTime;
    }

    public override string ClientName { get { return _client.Name; } }
    public override bool IsClientSuspicious { get { return _client.IsSuspicious; } }
    public override string AccountType { get { return "Deposit"; } }

    public override void WithdrawMoney(decimal sum, bool force = false)
    {
        if (Time.CurTime - _createdTime < _timeout && !force)
            throw new DepositMoneyWithdrawUnavailable(_number, _timeout - (Time.CurTime - _createdTime));
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