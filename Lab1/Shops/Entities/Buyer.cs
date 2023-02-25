using Shops.Models;

namespace Shops.Entities
{
    public class Buyer
    {
        private string _name;
        private decimal _account;
        private Guid _id;
        private Cart _cart;

        public string Name
        {
            get { return _name; }
        }

        public decimal Account
        {
            get { return _account; }
        }

        public Cart BuyerCart { get { return _cart; } }

        public Guid Id
        {
            get { return _id; }
        }

        public Buyer(string name)
        {
            _name = name;
            _account = 0;
            _id = Guid.NewGuid();
            _cart = new Cart();
        }

        public override string ToString()
        {
            return $"Buyer {_name} with ID {_id} has {_account} money";
        }

        public bool Equals(Buyer other)
        {
            return (this._id == other._id);
        }

        public void TopUpBalance(decimal sum)
        {
            _account += sum;
        }

        public void PayForOrder(decimal orderSum)
        {
            _account -= orderSum;
        }

        public void ClearCart()
        {
            _cart.Clear();
        }

        public void DisplayCart()
        {
            _cart.Display();
        }

        public void AddGoodToCart(IGood good)
        {
            _cart.AddGood(good);
        }

        public bool isAbleToPayForOrder() { return _cart.MaxOrderSum <= _account; }
    }
}
