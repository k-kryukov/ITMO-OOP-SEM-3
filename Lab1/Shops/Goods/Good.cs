using System;

namespace Shops.Models
{
    public class Good : GoodMixin, IGood
    {
        private Guid _id = Guid.NewGuid();

        public string Name
        {
            get { return _name; }
        }

        public Guid Id {
            get { return _id; }
        }

        public Good(string name, uint amount = 0, decimal pricePerOne = 0)
        {
            _name = name;
            _amount = amount;
            _pricePerOne = pricePerOne;
        }

        public Good(Good other) { _name = other.Name; _amount = other.Amount; _pricePerOne = other.PricePerOne; }

        public IGood Clone() { return new Good(this); }
        public IGood cloneDefault() { return new Good(_name); }
    }
}