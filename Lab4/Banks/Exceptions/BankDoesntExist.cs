namespace Banks.Exceptions;

public class BankDoesntExist : Exception
{
    public BankDoesntExist(string bankName)
    : base($"Bank {bankName} does not exist!") { }
}