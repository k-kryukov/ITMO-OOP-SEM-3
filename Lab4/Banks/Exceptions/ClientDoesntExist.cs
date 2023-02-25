namespace Banks.Exceptions;

public class ClientDoesntExist : Exception
{
    public ClientDoesntExist(string clientName)
    : base($"Client {clientName} does not exist!") { }
}