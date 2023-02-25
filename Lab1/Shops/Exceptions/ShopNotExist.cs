namespace Shops.Exceptions;

public class ShopNotExist : Exception
{
    public ShopNotExist(string shopName)
    : base($"Buyer {shopName} doesn't exists!") { }
}