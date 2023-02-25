using Banks.Models;
namespace Banks.Exceptions;

public class UnableToTransferBetweenAccounts : Exception
{
    public UnableToTransferBetweenAccounts(string srcClientName, AccountNumber srcAccountHash, string dstClientName, AccountNumber dstAccountHash, decimal sum)
    : base($"Unable to transfer {sum} from {srcAccountHash} (client {srcClientName}) to {dstAccountHash} (client {dstClientName})") { }
}