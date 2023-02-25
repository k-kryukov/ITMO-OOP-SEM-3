using System;

namespace Shops.Models
{
    public abstract class GoodMixin
    {
        protected uint _amount;
        protected decimal _pricePerOne;
        protected string _name = "";

        public uint Amount { get { return _amount; } }
        public decimal PricePerOne { get { return _pricePerOne; } }

        public void increaseAmount(uint numberOfAdded) { _amount += numberOfAdded; }
        public bool decreaseAmount(uint numberOfTaken)
        {
            if ((int)_amount - (int)numberOfTaken >= 0)
            {
                _amount -= numberOfTaken;
                return true;
            } else
            {
                return false;
            }
        }
        public void updatePrice(decimal newPrice)
        {
            _pricePerOne = newPrice;
        }

        public override string ToString()
        {
            return $"{_name}, {_amount} pieces, {_pricePerOne} for each";
        }
    }
}
