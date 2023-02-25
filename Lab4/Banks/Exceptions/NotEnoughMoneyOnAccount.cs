using Banks.Models;
namespace Banks.Exceptions;

public class NotEnoughMoneyOnAccount : Exception
{
    public NotEnoughMoneyOnAccount(AccountNumber number, decimal balance, decimal sum)
    : base($"Account {number} has only {balance}, when you try to get {sum}!") { }
}