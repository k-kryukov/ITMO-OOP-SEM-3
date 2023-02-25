using Banks.Models;
namespace Banks.Exceptions;

public class DepositMoneyWithdrawUnavailable : Exception
{
    public DepositMoneyWithdrawUnavailable(AccountNumber accountNumber, uint shift)
    : base($"Unable to get money from deposit {accountNumber} for {shift} days more!") { }
}