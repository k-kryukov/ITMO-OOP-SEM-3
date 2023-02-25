namespace Shops.Exceptions;

public class NotEnoughGoodsInShop : Exception
{
    public NotEnoughGoodsInShop(string shopName, string buyerName="Service Buyer")
    : base($"Shop {shopName} can not handle order from {buyerName}: not enough goods in shop!") { }
}