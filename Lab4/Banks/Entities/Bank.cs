using Banks.Accounts;
using Banks.Events;
using Banks.Exceptions;
using Banks.Models;
namespace Banks.Entities;

public class Bank : IBank
{
    private static decimal maxMoneyOnSuspiciousAccount = 200;
    private static decimal rateForHugeSum = 1;

    private string _name;
    private List<IClient> _clients;
    private decimal _debitPercents;
    private decimal _creditPercents;
    private decimal _maximumNegativeBalance;
    private SortedDictionary<decimal, decimal> _depositPercentsMapping;
    private Dictionary<IEvent, List<IClient>> _subscribedClients;

    public Bank(
        string bankName,
        decimal debitPercents,
        decimal creditPercents,
        SortedDictionary<decimal, decimal> depositPercentsMapping,
        decimal maximumNegativeBalance)
    {
        _name = bankName;
        _clients = new List<IClient>();
        _subscribedClients = new Dictionary<IEvent, List<IClient>>();
        _debitPercents = debitPercents;
        _depositPercentsMapping = depositPercentsMapping;
        _creditPercents = creditPercents;
        _maximumNegativeBalance = maximumNegativeBalance;
    }

    public string Name { get { return _name; } }

    public decimal CreditRate { get { return _creditPercents; } }
    public decimal DebitRate { get { return _debitPercents; } }
    public SortedDictionary<decimal, decimal> DepositPercentsMapping { get { return _depositPercentsMapping; } }

    public void SubscribeClient(string clientName, IEvent strategy)
    {
        var client = GetClientByName(clientName);

        if (_subscribedClients.ContainsKey(strategy))
        {
            _subscribedClients[strategy].Add(client);
        }
        else
        {
            _subscribedClients.Add(strategy, new List<IClient> { client });
        }
    }

    public void ChangeCreditRate(decimal newRate)
    {
        _creditPercents = newRate;
        NotifyClients(new CreditRateUpdated());
    }

    public void NotifyClients(CreditRateUpdated happenedEvent)
    {
        foreach (KeyValuePair<IEvent, List<IClient>> kvp in _subscribedClients)
        {
            if (!(kvp.Key is CreditRateUpdated))
                continue;

            foreach (var client in kvp.Value)
            {
                var strategy = kvp.Key;

                strategy = strategy.FillWithInfo(this, client);
                client.HandleEvent(strategy);
            }
        }
    }

    public AccountNumber CreateDebitAccount(string clientName)
    {
        var client = GetClientByName(clientName);
        var accountNumber = client.CreateDebitAccount(_debitPercents);
        return accountNumber;
    }

    public IAccount GetAccountByNumber(AccountNumber number)
    {
        foreach (var client in _clients)
        {
            try
            {
                var account = client.GetAccountByNumber(number);
                return account;
            }
            catch (AccountDoesntExist)
            {
            }
        }

        throw new AccountDoesntExist(number);
    }

    public AccountNumber CreateDepositAccount(string clientName, decimal balance)
    {
        var client = GetClientByName(clientName);
        var accountNumber = client.CreateDepositAccount(balance, _depositPercentsMapping, rateForHugeSum);
        return accountNumber;
    }

    public AccountNumber CreateCreditAccount(string clientName)
    {
        var client = GetClientByName(clientName);
        if (client.IsSuspicious)
            throw new CreditAccountNotAllowedForSuspiciousClient(clientName);
        var accountNumber = client.CreateCreditAccount(_creditPercents, _maximumNegativeBalance);
        return accountNumber;
    }

    public IClient GetClientByName(string clientName)
    {
        IClient desiredClient = (from client in _clients
                        where client.Name == clientName
                        select client).FirstOrDefault() ?? throw new ClientDoesntExist(clientName);
        return desiredClient;
    }

    public void AddClient(string clientName)
    {
        IClient? existingClient = (from client in _clients
                        where client.Name == clientName
                        select client).FirstOrDefault();
        if (existingClient != null)
            throw new ClientAlreadyExists(clientName);

        var newClient = new Client(clientName, this);
        _clients.Add(newClient);
    }

    public void DisplayInfo()
    {
        foreach (var client in _clients)
        {
            foreach (var account in client.Accounts)
            {
                Console.WriteLine(account);
            }
        }

        Console.WriteLine("__________________");
    }

    public TransactionNumber TopUpAccount(string clientName, AccountNumber accountNumber, decimal addedSum)
    {
        var client = GetClientByName(clientName);
        var account = client.GetAccountByNumber(accountNumber);
        if (client.IsSuspicious && account.Balance + addedSum > maxMoneyOnSuspiciousAccount)
            throw new TooManyMoneyOnSuspiciousAccount(account.Balance, addedSum);
        account.TopUpAccount(addedSum);

        ITransaction transaction = new Transaction(null, account.Number, addedSum, null, _name);
        CentralBank.GetInstance().SaveTransaction(transaction);
        return transaction.Id;
    }

    public TransactionNumber GetCash(string clientName, AccountNumber accountNumber, decimal sum)
    {
        var client = GetClientByName(clientName);
        var account = client.GetAccountByNumber(accountNumber);
        account.WithdrawMoney(sum);

        ITransaction transaction = new Transaction(account.Number, null, sum, _name, null);
        CentralBank.GetInstance().SaveTransaction(transaction);
        return transaction.Id;
    }

    public void SpecifyClientPersonalData(string clientName, PassportData passport, string address)
    {
        var client = GetClientByName(clientName);
        client.SpecifyPersonalData(passport, address);
    }

    public void ProcessPercents()
    {
        foreach (var client in _clients)
        {
            foreach (var account in client.Accounts)
            {
                account.ProcessPercents();
            }
        }
    }

    public TransactionNumber TransferBetweenAccounts(string srcClientName, AccountNumber srcAccountHash, string dstClientName, AccountNumber dstAccountHash, decimal sum)
    {
        var srcClient = GetClientByName(srcClientName);
        var dstClient = GetClientByName(dstClientName);

        var srcAccount = srcClient.GetAccountByNumber(srcAccountHash);
        var dstAccount = dstClient.GetAccountByNumber(dstAccountHash);
        if (srcAccount.IsAbleToTransfer(sum))
        {
            srcAccount.WithdrawMoney(sum);
            dstAccount.TopUpAccount(sum);

            ITransaction transaction = new Transaction(srcAccountHash, dstAccountHash, sum, _name, _name);
            CentralBank.GetInstance().SaveTransaction(transaction);
            return transaction.Id;
        }
        else
        {
            throw new UnableToTransferBetweenAccounts(srcClientName, srcAccountHash, dstClientName, dstAccountHash, sum);
        }
    }

    public void CancelSrcPay(AccountNumber number, decimal sum)
    {
        var srcAccount = GetAccountByNumber(number);
        srcAccount.TopUpAccount(sum);
    }

    public void CancelDstPay(AccountNumber number, decimal sum)
    {
        var dstAccount = GetAccountByNumber(number);
        dstAccount.WithdrawMoney(sum, true);
    }

    public override string ToString()
    {
        return _name;
    }
}