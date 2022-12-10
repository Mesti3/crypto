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
            SupportedOperations.Add(3, Sell);         
            SupportedOperations.Add(5, GetDbOrdersProfit);
            SupportedOperations.Add(6, GetAllOpenOrdersProfit);
            SupportedOperations.Add(7, GetWalletStatus);
            SupportedOperations.Add(8, GetAssets);
            SupportedOperations.Add(99, DoSomething);
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
                var quantity =console.ReadLine();
                decimal? quantityValue = null;
                if (!string.IsNullOrWhiteSpace(quantity))
                    quantityValue = decimal.Parse(quantity);
                console.WriteLine("insert total value");
                var price = console.ReadLine();
                decimal? priceValue = null;
                if (!string.IsNullOrWhiteSpace(price))
                    priceValue = decimal.Parse(price);
                var order = TradeController.BuyAsync(symbol, quantityValue, priceValue).Result;
                console.WriteLine("created purchase:");
                console.WriteLine(String.Format("id: {0}\tsymbol: {1}", order.ClientOrderId,order.Symbol));
                console.WriteLine(String.Format("quantity: {0}\tprice:{1}", order.Quantity, order.TotalPrice));
                console.WriteLine(String.Format("trade quantity: {0}\ttrade price:{1}", order.Trades.Sum(x=>x.Quantity), order.Trades.Sum(x=>x.Quantity * x.Price)));

            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString());
            }
            console.WriteLine();
        }
        private void Sell()
        {
            try
            {
                console.WriteLine("insert symbol");
                string symbol = console.ReadLine();
                console.WriteLine("insert quantity");
                var quantity = console.ReadLine();
                decimal? quantityValue = null;
                if (!string.IsNullOrWhiteSpace(quantity))
                    quantityValue = decimal.Parse(quantity);
                console.WriteLine("insert total value");
                var price = console.ReadLine();
                decimal? priceValue = null;
                if (!string.IsNullOrWhiteSpace(price))
                    priceValue = decimal.Parse(price);
                var order = TradeController.SellAsync(symbol, quantityValue, priceValue).Result;
                console.WriteLine("created sale:");
                console.WriteLine(String.Format("id: {0}\tsymbol: {1}", order.ClientOrderId, order.Symbol));
                console.WriteLine(String.Format("quantity: {0}\tprice:{1}", order.Quantity, order.TotalPrice));
                console.WriteLine(String.Format("trade quantity: {0}\ttrade price:{1}", order.Trades.Sum(x => x.Quantity), order.Trades.Sum(x => x.Quantity * x.Price)));

            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString());
            }
            console.WriteLine();
        }
      
        private void GetDbOrdersProfit()
        {
            try
            {
                var orders = TradeController.GetDBOrdersProfit().Result;
                foreach (var order in orders)
                {
                    console.WriteLine(String.Format("id: {0}\tsymbol: {1:F5}\tcreated:{2}\tunitprice:{3:F5}\tth.unitprice:{4:F5}\tprice:{5:F5}\tth.price:{6:F5}\tprofit:{7:F5}\tprofit%:{8:P5}", order.ExternalOrderId, order.Symbol, order.CreateTime, order.UnitPrice, order.ActualUnitPrice, order.TotalPrice, order.TheoreticalPrice, order.Profit, order.ProfitRatio));
                }
            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString());
            }
            console.WriteLine();
        }
        private void GetAllOpenOrdersProfit()
        {
            try
            {
                console.WriteLine("insert symbol");
                string symbol = console.ReadLine();
                var orders = TradeController.GetAllOrdersProfit(symbol).Result;
                

                console.WriteLine("profit:");
                foreach (var order in orders)
                {
                    console.WriteLine(String.Format("id: {0}\tsymbol: {1:F5}\tcreated:{2}\tunitprice:{3:F5}\tth.unitprice:{4:F5}\tprice:{5:F5}\tth.price:{6:F5}\tprofit:{7:F5}\tprofit%:{8:P5}", order.ClientOrderId, order.Symbol, order.CreateTime,order.UnitPrice, order.ActualUnitPrice, order.TotalPrice, order.TheoreticalPrice, order.Profit, order.ProfitRatio));
                }
               
            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString());
            }
            console.WriteLine();
        }
        private void GetWalletStatus()
        {
            try
            {
                var asset = TradeController.GetWalletStatusAsync().Result;
                console.WriteLine(String.Format("symbol: {0}\tavailable:{1:F5}\ttotal:{2:F5}", asset.Symbol, asset.Available, asset.Total));
            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString());
            }
            console.WriteLine();

        }
        private void GetAssets()
        {
            try
            {
                var assets = TradeController.GetAssetsAsync().Result;
                assets.ForEach(asset => console.WriteLine(String.Format("symbol: {0}\tavailable:{1:F5}\ttotal:{2:F5}", asset.Symbol, asset.Available, asset.Total)));
            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString());
            }
            console.WriteLine();

        }
        private void DoSomething()
        {
            try
            {
                TradeController.DoSomethingAsync("ETHBTC",0);


                console.WriteLine("done:");
        
               
            }
            catch (Exception ex)
            {
                console.WriteLine(ex.ToString());
            }
            console.WriteLine();
        }

    }
}
//api-key: PtQO1VvdQUKPxJ6qe2yZruTLjoBvqc8EszVlEIhUUHyGpF3Z924riwqxJVS5t5yi
//secret-key: CXZohJ2jOUln1U9ZZcqft5Kt9jzbsuWcsAI2mn6E4EABBEW4qRuh82p4eK5BzmQy