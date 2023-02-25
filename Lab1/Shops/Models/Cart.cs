namespace Shops.Models
{
    public class Cart
    {
        private List<IGood> _goodsList;
        private decimal _maxOrderSum;

        public decimal MaxOrderSum { get { return _maxOrderSum; } }

        public void AddGood(IGood addedGood)
        {
                IGood? oldGood = (from good in _goodsList
                                where good.Name == addedGood.Name
                                select good
                                ).FirstOrDefault();
                if (oldGood == null)
                {
                    _maxOrderSum += (addedGood.Amount * addedGood.PricePerOne);
                    _goodsList.Add(addedGood.Clone());
                }
                else
                {
                    _maxOrderSum -= (oldGood.Amount * oldGood.PricePerOne);
                    if (oldGood.PricePerOne > addedGood.PricePerOne)
                    {
                        oldGood.updatePrice(addedGood.PricePerOne);
                        oldGood.increaseAmount(addedGood.Amount);
                        _maxOrderSum += (oldGood.Amount * oldGood.PricePerOne);
                    }
                }
        }

        public void Display()
        {
            Console.WriteLine("Cart consists from:");
            foreach (var good in _goodsList)
            {
                Console.WriteLine(good);
            }
        }

        public void Clear()
        {
            _goodsList.Clear();
        }

        public Cart() { _goodsList = new List<IGood>(); _maxOrderSum = 0; }

        public IList<IGood> Goods
        {
            get { return _goodsList.AsReadOnly(); }
        }
    }
}
