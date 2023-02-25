namespace Banks.Exceptions;

public class TooManyMoneyOnSuspiciousAccount : Exception
{
    public TooManyMoneyOnSuspiciousAccount(decimal balance, decimal addedSum)
    : base($"Is is not allowed to add {addedSum} to account with balance {balance}, because account owner is Suspicious") { }
}