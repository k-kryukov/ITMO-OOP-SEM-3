namespace Shops.Exceptions;

public class BuyerNotExist : Exception
{
    public BuyerNotExist(string buyerName)
    : base($"Buyer {buyerName} doesn't exists!") { }
}