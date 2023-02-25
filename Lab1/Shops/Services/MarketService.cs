using Shops.Entities;
using Shops.Models;
using Shops.Exceptions;
using System;

namespace Shops.Services
{
    public class MarketService : IMarketService
    {
        private List<Buyer> _buyers;
        private List<Shop> _shops;

        public IList<Buyer> Buyers
        {
            get { return _buyers.AsReadOnly(); }
        }

        public IList<Shop> Shops
        {
            get { return _shops.AsReadOnly(); }
        }

        public Buyer GetBuyer(string name)
        {
            Buyer foundBuyer = (_buyers.Find(buyer => buyer.Name == name) ?? throw new BuyerNotExist(name));
            return foundBuyer;
        }

        public Shop GetShop(string name)
        {
            Shop foundShop = (_shops.Find(shop => shop.Name == name) ?? throw new ShopNotExist(name));
            return foundShop;
        }

        public MarketService()
        {
            _buyers = new List<Buyer>();
            _shops = new List<Shop>();
        }

        public void MakeOrder(string buyerName, Shop shop)
        {
            Buyer foundBuyer = (_buyers.Find(buyer => buyer.Name == buyerName) ?? throw new BuyerNotExist(buyerName));

            if (!foundBuyer.isAbleToPayForOrder())
            {
                throw new NotEnoughMoney(foundBuyer.Name);
            }

            var orderSum = shop.TryHandleOrder(foundBuyer.BuyerCart, foundBuyer.Name);

            foundBuyer.PayForOrder(orderSum);
            Console.WriteLine($"Successfully handled order {foundBuyer.BuyerCart} in {shop}");

        }

        public void ChangeGoodPrice(Shop shop, IGood good, decimal newPrice)
        {
            var oldPrice = shop.changePrice(good.cloneDefault(), newPrice);
            Console.WriteLine($"Successfully changed {good.Name} price to {newPrice}");
        }

        public Shop? findTheBestStore(IGood wantedGood)
        {
            decimal? minPossiblePrice = null;
            Shop? theBestStore = null;

            foreach (var shop in _shops)
            {
                decimal? curPossiblePrice = shop.countOrderSum(wantedGood);
                if (minPossiblePrice == null || curPossiblePrice < minPossiblePrice)
                {
                    minPossiblePrice = curPossiblePrice;
                    theBestStore = shop;
                }
            }

            return theBestStore;
        }

        public Shop AddShop(string name, string address)
        {
            var shop = new Shop(name, address);
            _shops.Add(shop);
            return shop;
        }

        public void AddGoodToCart(string buyerName, IGood good)
        {
            Buyer foundBuyer = (_buyers.Find(buyer => buyer.Name == buyerName) ?? throw new BuyerNotExist(buyerName));
            foundBuyer.AddGoodToCart(good);
        }

        public void TopUpBalance(string buyerName, decimal money)
        {
            var buyer = GetBuyer(buyerName);
            buyer.TopUpBalance(money);
        }

        public Buyer AddBuyer(string name)
        {
            var buyer = new Buyer(name);
            _buyers.Add(buyer);
            return buyer;
        }

        public void AddGoodsToShop(Shop shop, HashSet<IGood> supply)
        {
            shop.AddNewGoods(supply);
        }

        public void DisplayAssortment()
        {
            Console.WriteLine("Current assortment is: ");
            foreach (var shop in _shops)
            {
                foreach (var good in shop.Stock)
                {
                    Console.WriteLine($"{shop}: {good}");
                }
            }
        }

        public void DisplayRegistredBuyers()
        {
            Console.WriteLine("Currently registred buyers are: ");

            foreach (var buyer in _buyers)
            {
                Console.WriteLine(buyer);
            }
        }
    }
}
