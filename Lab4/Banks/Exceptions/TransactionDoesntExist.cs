using Banks.Models;
namespace Banks.Exceptions;

public class TransactionDoesntExist : Exception
{
    public TransactionDoesntExist(TransactionNumber transactionNumber)
    : base($"Transaction {transactionNumber} does not exist!") { }
}