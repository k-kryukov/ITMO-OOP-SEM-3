using System;
using Shops.Services;
using Shops.Entities;
using Shops.Models;
using Shops.Exceptions;

namespace A
{
    public class Program
    {
        public static void printGeneralHint() { Console.WriteLine("0 - exit, 1 - get info, 2 - other"); }

        public static void handleInfoOption(MarketService service)
        {
            Console.WriteLine("0 - back to main menu, 1 - get buyers list, 2 - get shops list, 3 - get assortment");
            int opt = Convert.ToInt32(Console.ReadLine());

            if (opt == 0) { return; }
            else if (opt == 1)
            {
                service.DisplayRegistredBuyers();
            }
            else if (opt == 2)
            {
                Console.WriteLine("Registred shops are: ");
                foreach (var shop in service.Shops)
                {
                    Console.WriteLine(shop);
                }
            }
            else if (opt == 3)
            {
                service.DisplayAssortment();
            }
        }

        public static void handleChangeOption(MarketService service)
        {
            Console.WriteLine("0 - back to main menu, 1 - add buyer, 2 - top up buyer balance, " +
                              "3 - add a shop, 4 - add good to a shop, 5 - add good to a cart, 6 - request order handling, " +
                              "7 - change a good price, 8 - find the best store to buy a good");
            int opt = Convert.ToInt32(Console.ReadLine());

            if (opt == 0) { return; }
            else if (opt == 1)
            {
                Console.WriteLine("Input buyer name!");
                var buyerName = Console.ReadLine() ?? throw new Exception();

                service.AddBuyer(buyerName);
            }
            else if (opt == 2)
            {
                Console.WriteLine("Input a buyer name");
                var buyerName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input a sum that you want top up balance for");
                decimal topUpSum = Convert.ToUInt32(Console.ReadLine());

                try { service.TopUpBalance(buyerName, topUpSum); }
                catch (BuyerNotExist ex) { Console.WriteLine(ex); }
            }
            else if (opt == 3)
            {
                Console.WriteLine("Input a shop name");
                var shopName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input a shop address");
                var shopAddress = Console.ReadLine() ?? throw new Exception();

                service.AddShop(shopName, shopAddress);
            }
            else if (opt == 4)
            {
                Console.WriteLine("Input a shop name");
                var shopName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input a good name");
                var goodName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input an amount");
                var amount = Convert.ToUInt32(Console.ReadLine());

                Console.WriteLine("Input a supply price");
                var minimumPrice = Convert.ToUInt32(Console.ReadLine());

                IGood goodObject = new Good(goodName, amount, minimumPrice);
                var supply = new HashSet<IGood> { goodObject };

                try { service.AddGoodsToShop(service.GetShop(shopName), supply); }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            else if (opt == 5)
            {
                Console.WriteLine("Input a buyer name");
                var buyerName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input a good name");
                var goodName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input an amount");
                var amount = Convert.ToUInt32(Console.ReadLine());

                Console.WriteLine("Input a maximum price that you are satisfied with");
                var minimumPrice = Convert.ToUInt32(Console.ReadLine());

                IGood goodObject = new Good(goodName, amount, minimumPrice);
                if (goodObject == null) { Console.WriteLine("Wrong good name!"); return; }
                try { service.AddGoodToCart(buyerName, goodObject); }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            else if (opt == 6)
            {
                Console.WriteLine("Input a buyer name to send stored cart info!");
                var buyerName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input a shop where you want to request buyer's cart handling");
                var shopName = Console.ReadLine() ?? throw new Exception();

                try { service.MakeOrder(buyerName, service.GetShop(shopName)); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            else if (opt == 7)
            {
                Console.WriteLine("Input a good name!");
                var goodName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input a shop where you want to change good's price!");
                var shopName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input a new price for specified good");
                var newPrice = Convert.ToUInt32(Console.ReadLine());

                IGood goodObject = new Good(goodName);
                if (goodObject == null) { Console.WriteLine("Wrong good name!"); return; }
                try { service.ChangeGoodPrice(service.GetShop(shopName), goodObject, newPrice); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            else if (opt == 8)
            {
                Console.WriteLine("Input a good name!");
                var goodName = Console.ReadLine() ?? throw new Exception();

                Console.WriteLine("Input an amount of specified good you are looking for");
                var goodAmount = Convert.ToUInt32(Console.ReadLine());

                IGood goodObject = new Good(goodName, goodAmount);
                if (goodObject == null) { Console.WriteLine("Wrong good name!"); return; }

                Shop? theBestStore = service.findTheBestStore(goodObject);
                if (theBestStore == null)
                    Console.WriteLine("No shops have been registred yet!");
                else
                    Console.WriteLine($"The best store is {theBestStore}");
            }
        }

        public static void Main(string[] args)
        {
            var service = new MarketService();

            service.AddBuyer("daria");
            service.TopUpBalance("daria", 100000);

            var shop = service.AddShop("dixi", "26th line VI, 7");
            service.AddGoodsToShop(shop, new HashSet<IGood> {new Good("Carrot", 200, 5)} );
            service.AddGoodsToShop(shop, new HashSet<IGood> {new Good("CocaCola", 80, 50)} );

            // service.AddGoodToCart("k.kryukov", new CocaCola(80, 90));
            // service.MakeOrder("k.kryukov", service.GetShop("dixi"));

            // service.DisplayAssortment();
            // service.DisplayRegistredBuyers();

            while (true)
            {
                printGeneralHint();

                int opt = Convert.ToInt32(Console.ReadLine());
                if (opt == 0) { return; }
                else if (opt == 1) { handleInfoOption(service); }
                else if (opt == 2) { handleChangeOption(service); }
                else { Console.WriteLine("Wrong option number!"); }
            }

            // service.TopUpBalance("k.kryukov", 1000);
            // service.AddGoodsToShop(dixi, new HashSet<IGood> { new Potato(300, 2), new CocaCola(100, 50) });
            // service.AddGoodsToShop(dixi, new HashSet<IGood> { new Carrot(300, 5) });

            // service.AddGoodsToShop(magnit, new HashSet<IGood> { new Potato(300, 1), new CocaCola(100, 40) });
            // service.AddGoodsToShop(magnit, new HashSet<IGood> { new Carrot(300, 4) });

            // service.AddGoodToCart("k.kryukov", new Carrot(10, 5));
            // service.AddGoodToCart("k.kryukov", new CocaCola(2, 70));

            // service.DisplayAssortment();

            // service.DisplayRegistredBuyers();
            // service.MakeOrder("k.kryukov", dixi);
            // service.DisplayRegistredBuyers();

            // Console.WriteLine("");
            // service.DisplayAssortment();
            // Console.WriteLine("");
            // buyer.BuyerCart.Display();

        }
    }
}