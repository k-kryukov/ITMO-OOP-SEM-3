namespace Banks.Exceptions;

public class BankAlreadyExists : Exception
{
    public BankAlreadyExists(string bankName)
    : base($"Bank {bankName} already exists!") { }
}