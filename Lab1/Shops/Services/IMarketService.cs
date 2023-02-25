using Shops.Entities;
using Shops.Models;
using System;

namespace Shops.Services
{
    public interface IMarketService
    {
        void MakeOrder(string buyerName, Shop shop);

        Shop AddShop(string name, string address);

        void TopUpBalance(string buyerName, decimal money);

        Buyer AddBuyer(string name);

        void AddGoodsToShop(Shop shop, HashSet<IGood> supply);
    }
}
