namespace Banks.Exceptions;

public class ClientAlreadyExists : Exception
{
    public ClientAlreadyExists(string clientName)
    : base($"Client {clientName} already exists!") { }
}