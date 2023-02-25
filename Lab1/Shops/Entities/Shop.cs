using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shops.Exceptions;

namespace Shops.Models
{
    public class Shop
    {
        private static double priceOverhead = 1.5;

        private string _name;
        private string _address;
        private static Guid _id = Guid.NewGuid();
        private HashSet<IGood> stock;

        public string Name { get { return _name; } }
        public Guid Id { get { return _id; } }
        public HashSet<IGood> Stock { get { return stock; } }

        public Shop(string name, string address)
        {
            _name = name;
            _address = address;
            stock = new HashSet<IGood>();
        }

        public void AddNewGoods(in HashSet<IGood> supply)
        {
            foreach (var newGood in supply)
            {
                var addedGood = newGood.Clone();
                addedGood.updatePrice((int)((double)newGood.PricePerOne * priceOverhead));

                IGood? oldGood = (from good in stock
                                where good.Name == newGood.Name
                                select good
                                ).FirstOrDefault();
                if (oldGood == null)
                {
                    addedGood.updatePrice((int)((double)newGood.PricePerOne * priceOverhead));
                    stock.Add(addedGood);
                }
                else
                {
                    oldGood.increaseAmount(addedGood.Amount);
                    if (oldGood.PricePerOne < addedGood.PricePerOne)
                        oldGood.updatePrice(addedGood.PricePerOne);
                }
            }
        }

        public decimal TryHandleOrder(Cart cart, string buyerName)
        {
            foreach (var requestedGood in cart.Goods)
            {
                IGood? stockGood = (from good in stock
                                where good.Name == requestedGood.Name
                                select good
                                ).FirstOrDefault();
                if (stockGood == null || stockGood.Amount < requestedGood.Amount)
                    throw new NotEnoughGoodsInShop(Name, buyerName);
                if (stockGood.PricePerOne > requestedGood.PricePerOne)
                    throw new GoodIsTooExpensive(this.Name, stockGood, requestedGood.PricePerOne);
            }

            return handleOrder(cart);
        }

        public bool changePrice(IGood good, decimal newPrice)
        {
            IGood? foundGood = (from curGood in stock
                            where curGood.Name == good.Name
                            select curGood
                            ).FirstOrDefault();
            if (foundGood == null)
                return false;
            else
            {
                foundGood.updatePrice(newPrice);
                return true;
            }
        }

        public decimal handleOrder(Cart cart)
        {
            decimal orderSum = 0;
            foreach (var requestedGood in cart.Goods)
            {
                IGood stockGood = (from good in stock
                                where good.Name == requestedGood.Name
                                select good
                                ).First();

                orderSum += requestedGood.Amount * stockGood.PricePerOne;
                stockGood.decreaseAmount(requestedGood.Amount);
            }

            return orderSum;
        }

        public decimal? countOrderSum(IGood good)
        {
            IGood? foundGood = (from curGood in stock
                            where curGood.Name == good.Name
                            select curGood
                            ).FirstOrDefault();
            if (foundGood == null)
                return null;
            else
            {
                if (foundGood.Amount > good.Amount)
                    return good.Amount * foundGood.PricePerOne;
                else
                    return null;
            }
        }

        public override string ToString()
        {
            return $"Shop {_name} located in {_address}";
        }
    }
}
