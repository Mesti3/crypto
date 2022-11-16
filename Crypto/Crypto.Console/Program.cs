using Crypto;

namespace CryptoConsole
{
    internal class Program
    {
        private static TradeController TradeController;
        static void Main(string[] args)
        {
            TradeController = new TradeController();
            
            while (true)
            {
                Console.WriteLine("select operation \n\tBUY = 1");
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.Escape: return;
                    case ConsoleKey.NumPad1: Buy();break ;
                }
            }
        }

     

        private static void Buy()
        {
            try
            {
                
                Console.WriteLine("insert symbol");
                string symbol = Console.ReadLine();
                Console.WriteLine("insert quantity");
                decimal quantity = decimal.Parse(Console.ReadLine());
                Console.WriteLine("insert value");
                var price = Console.ReadLine();
                decimal? priceValue = null;
                if (!string.IsNullOrWhiteSpace(price))
                    priceValue = decimal.Parse(price);
                if (TradeController.BuyAsync(symbol, quantity, priceValue).Result)
                    Console.WriteLine("order created");
                else
                    Console.WriteLine("error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString);
            }
        }
    }
}