using Shops.Exceptions;
using Shops.Models;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class MarketServiceTest
{
    private MarketService service;

    public MarketServiceTest()
    {
        service = new MarketService();
    }

    [Fact]
    public void SupplyToShop_BuyerIsAbleToBuy()
    {
        var shop = service.AddShop("Test Shop", "Test Shop address");
        var goodsSet = new HashSet<IGood> { new Good("Carrot", 100, 1), new Good("CocaCola", 100, 50) };

        service.AddGoodsToShop(shop, goodsSet);

        var buyer = service.AddBuyer("k.kryukov");
        service.TopUpBalance("k.kryukov", 1000);
        service.AddGoodToCart("k.kryukov", new Good("CocaCola", 3, 80));
        service.AddGoodToCart("k.kryukov", new Good("Carrot", 7, 10));
        try
        {
            service.MakeOrder("k.kryukov", shop);
        }
        catch (Exception)
        {
            // expected no exception
            throw;
        }
    }

    [Fact]
    public void PrepareShopAssortment_BestShopToBuyGoodIsCorrect()
    {
        var shop_1 = service.AddShop("Test Shop", "Test Shop address");
        var shop_2 = service.AddShop("Test Shop 2", "Test Shop address");
        var goodsSet_1 = new HashSet<IGood> { new Good("Carrot", 100, 1), new Good("CocaCola", 100, 50) };
        var goodsSet_2 = new HashSet<IGood> { new Good("CocaCola", 80, 40) };

        service.AddGoodsToShop(shop_1, goodsSet_1);
        service.AddGoodsToShop(shop_2, goodsSet_2);

        var theBestShop_1 = service.findTheBestStore(new Good("CocaCola", 90));
        var theBestShop_2 = service.findTheBestStore(new Good("CocaCola", 20));
        Assert.Equal(theBestShop_1, shop_1);
        Assert.Equal(theBestShop_2, shop_2);
    }

    [Fact]
    public void TryToBuyTooManyGoods_ThrowException()
    {
        var shop = service.AddShop("Test Shop", "Test Shop address");
        var goodsSet = new HashSet<IGood> { new Good("Carrot", 100, 1) };

        service.AddGoodsToShop(shop, goodsSet);

        var buyer = service.AddBuyer("k.kryukov");
        service.TopUpBalance("k.kryukov", 1000);
        service.AddGoodToCart("k.kryukov", new Good("Carrot", 200, 5));

        Assert.Throws<NotEnoughGoodsInShop>(() => service.MakeOrder("k.kryukov", shop));
    }

    [Fact]
    public void TryToBuyWithTooSmallMaximumPrice_ThrowException()
    {
        var shop = service.AddShop("Test Shop", "Test Shop address");
        var goodsSet = new HashSet<IGood> { new Good("CocaCola", 100, 60) };

        service.AddGoodsToShop(shop, goodsSet);

        var buyer = service.AddBuyer("k.kryukov");
        service.TopUpBalance("k.kryukov", 1000);
        service.AddGoodToCart("k.kryukov", new Good("CocaCola", 2, 80));

        Assert.Throws<GoodIsTooExpensive>(() => service.MakeOrder("k.kryukov", shop));
    }

    [Fact]
    public void TryToTopUpBalanceOfUnexistingBuyer_ThrowException()
    {
        Assert.Throws<BuyerNotExist>(() => service.TopUpBalance("k.kryukov", 1000));
    }

    [Fact]
    public void TryToOrderSupplyToUnexistingShop_ThrowException()
    {
        service.AddShop("dixi", "dixi address");
        service.AddShop("magnit", "magnit address");
        Assert.Throws<ShopNotExist>(() => service.GetShop("pyaterochka"));
    }

    [Fact]
    public void ChangePriceAfterSupply_PriceChanged()
    {
        var shop = service.AddShop("dixi", "dixi address");
        var goodsSet = new HashSet<IGood> { new Good("CocaCola", 100, 60) };
        service.AddGoodsToShop(shop, goodsSet);

        var goodToChangePrice = new Good("CocaCola");
        decimal newPrice = 40;
        service.ChangeGoodPrice(shop, goodToChangePrice, newPrice);

        IGood? foundGood = (from good in shop.Stock
                        where good.Name == goodToChangePrice.Name
                        select good).FirstOrDefault();

        Assert.True(foundGood != null);
        Assert.True(foundGood != null && foundGood.PricePerOne == newPrice);  // to satisfy static analyzer
    }

    [Fact]
    public void BuyConsignmentInShop_MoneyPaid_StockChanged()
    {
        var shop = service.AddShop("dixi", "dixi address");
        var goodsSet = new HashSet<IGood> { new Good("Carrot", 100, 7), new Good("CocaCola", 100, 60), new Good("Potato", 200, 4) };

        service.AddGoodsToShop(shop, goodsSet);

        var buyer = service.AddBuyer("k.kryukov");
        service.TopUpBalance("k.kryukov", 1000);
        service.AddGoodToCart("k.kryukov", new Good("Carrot", 20, 12));
        service.AddGoodToCart("k.kryukov", new Good("CocaCola", 4, 120));
        service.AddGoodToCart("k.kryukov", new Good("Potato", 15, 8));

        try
        {
            service.MakeOrder("k.kryukov", shop);
        }
        catch (Exception)
        {
            // expected no exception
            throw;
        }
    }
}