using System;

namespace Shops.Models
{
    public interface IGood
    {
        string Name { get; }
        uint Amount { get; }
        decimal PricePerOne { get; }
        Guid Id { get; }

        void increaseAmount(uint numberOfAdded);
        bool decreaseAmount(uint numberOfTaken);
        void updatePrice(decimal newPrice);
        IGood Clone();
        IGood cloneDefault();
        string ToString();
    }
}
