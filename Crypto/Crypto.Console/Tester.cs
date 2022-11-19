using Crypto.Model.ServiceEntities;
using console = System.Console;
namespace Crypto.Console
{
    internal class Tester
    {
        private TradeController TradeController;
        private Dictionary<int, Action> SupportedOperations;
        private bool Cancelled;
        public void Run()
        {
            TradeController = new TradeController();
            FillSupportedOperations();
            Cancelled = false;

            while (!Cancelled)
            {
                console.WriteLine("select operation:");
                WriteOperations();

                var key = console.ReadLine();
                console.WriteLine();

                if (int.TryParse(key, out int index))
                if (SupportedOperations.ContainsKey(index))
                    SupportedOperations[index].Invoke();
            }
        }

         private  void FillSupportedOperations()
        {
            SupportedOperations = new Dictionary<int, Action>(2);
            SupportedOperations.Add(0, Exit);
            SupportedOperations.Add(1, GetActualPrices);
            SupportedOperations.Add(2, Buy);
        }
        private void WriteOperations()
        {
            foreach (var operation in SupportedOperations)
                console.WriteLine(String.Format("{0} - {1}", operation.Key, operation.Value.Method.Name));
        }

        public void Exit()
        {
            Cancelled = true;
        }
        private void GetActualPrices()
        {
            try
            {
                console.WriteLine("insert symbol (or more, separated by coma)");
                string line = console.ReadLine();
                List<ActualPrice>? result = null;
                if (string.IsNullOrWhiteSpace(line))
                    result = TradeController.GetActualPrices().Result;
                else
                    result = TradeController.GetActualPrices(line.Split(',')).Result;
                result.ForEach(x => console.WriteLine(x.ToString()));
            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString());
            }
            console.WriteLine();
        }
        private void Buy()
        {
            try
            {
                console.WriteLine("insert symbol");
                string symbol = console.ReadLine();
                console.WriteLine("insert quantity");
                decimal quantity = decimal.Parse(console.ReadLine());
                console.WriteLine("insert value");
                var price = console.ReadLine();
                decimal? priceValue = null;
                if (!string.IsNullOrWhiteSpace(price))
                    priceValue = decimal.Parse(price);
                if (TradeController.BuyAsync(symbol, quantity, priceValue).Result)
                    console.WriteLine("order created");
                else
                    console.WriteLine("error");
            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString);
            }
            console.WriteLine();
        }
    }
}
