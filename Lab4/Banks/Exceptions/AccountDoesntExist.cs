using Banks.Models;
namespace Banks.Exceptions;

public class AccountDoesntExist : Exception
{
    public AccountDoesntExist(AccountNumber accountNumber)
    : base($"Account {accountNumber} does not exist!") { }
}