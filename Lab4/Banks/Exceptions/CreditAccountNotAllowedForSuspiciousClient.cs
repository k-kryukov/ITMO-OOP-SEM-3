namespace Banks.Exceptions;

public class CreditAccountNotAllowedForSuspiciousClient : Exception
{
    public CreditAccountNotAllowedForSuspiciousClient(string clientName)
    : base($"Client {clientName} is suspicious (passport/address aren't specified), so credit account is not allowed!") { }
}