using Banks.Models;
namespace Banks.Exceptions;

public class UnableToTransferBetweenBanks : Exception
{
    public UnableToTransferBetweenBanks(string srcBankName, string dstBankName, AccountNumber srcAccountHash, AccountNumber dstAccountHash, decimal sum)
    : base($"Unable to transfer {sum} from {srcAccountHash} (bank {srcBankName}) to {dstAccountHash} (bank {dstBankName})") { }
}