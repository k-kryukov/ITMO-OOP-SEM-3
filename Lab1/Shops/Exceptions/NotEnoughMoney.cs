namespace Shops.Exceptions;

public class NotEnoughMoney : Exception
{
    public NotEnoughMoney(string buyerName)
    : base($"Buyer {buyerName} does not have enough money to pay for his order!") { }
}