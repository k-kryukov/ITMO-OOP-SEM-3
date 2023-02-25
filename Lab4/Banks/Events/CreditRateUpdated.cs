using System;
using Banks.Entities;
namespace Banks.Events;

public class CreditRateUpdated : IEvent
{
    private static Guid _id = Guid.NewGuid();
    private decimal _newRate = 0;
    private string _bankName = string.Empty;
    private string _clientName = string.Empty;

    public CreditRateUpdated(string bankName, decimal newRate, string clientName)
    {
        _newRate = newRate;
        _bankName = bankName;
        _clientName = clientName;
    }

    public CreditRateUpdated()
    {
    }

    public Guid GetEventId() { return _id; }

    public IEvent CloneWithInfo(string bankName, decimal newRate, string clientName)
    {
        return new CreditRateUpdated(bankName, newRate, clientName);
    }

    public void LogUpdate()
    {
        Console.WriteLine($"[NOTIFICATION][{_clientName}] Credit rate was updated in {_bankName} bank - now it is {_newRate}");
    }

    public IEvent FillWithInfo(IBank bank, IClient client)
    {
        return CloneWithInfo(bank.Name, bank.CreditRate, client.Name);
    }
}