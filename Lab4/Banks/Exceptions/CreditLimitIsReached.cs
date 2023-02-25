using Banks.Models;
namespace Banks.Exceptions;

public class CreditLimitIsReached : Exception
{
    public CreditLimitIsReached(decimal sum, decimal maxNegativeBalance, AccountNumber accountNumber)
    : base($"Unable to get {sum} from account {accountNumber}: {maxNegativeBalance} is maximum negative balance!") { }
}