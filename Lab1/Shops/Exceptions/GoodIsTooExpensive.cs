using Shops.Models;

namespace Shops.Exceptions;

public class GoodIsTooExpensive : Exception
{
    public GoodIsTooExpensive(string shopName, IGood good, decimal maxPrice)
    : base($"Shop {shopName} has {good.Name} only for {good.PricePerOne} when buyer's maximum price is {maxPrice}") { }
}