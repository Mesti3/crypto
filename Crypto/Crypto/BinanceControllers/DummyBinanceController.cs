using Crypto.Model;
using Crypto.Model.Entities;

namespace Crypto.BinanceControllers
{
    internal class DummyBinanceController :IBinanceController
    {

        public async Task<Purchase> Buy(string symbol, decimal quantity, decimal price)
        {
            Random r = new Random();
            if (r.Next(5) != 0)
                return GetDummyPurchase(symbol, quantity,price);
            else
                throw new Exception("BUY: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\", \"price\"" + price + "\"}", new Exception("Dummy error message"));

        }
        public async Task<Purchase> Buy(string symbol, decimal quantity)
        {
            Random r = new Random();
            if (r.Next(5) != 0)
                return GetDummyPurchase(symbol, quantity, r.Next(1000,1000000)*0.01M);
            else
                throw new Exception("BUY: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\"}", new Exception("Dummy error message"));
            
        }

        private Purchase GetDummyPurchase(string symbol, decimal quantity, decimal price)
        {
            return new Purchase()
            {
                ClientOrderId = ClientOrderId.GetNew(symbol),
                Symbol = symbol,
                CreateTime = DateTime.Now,
                Price = price,
                Quantity = quantity,
                Trades = new List<PurchaseTrade>() {
                        new PurchaseTrade {
                            TradeId = 1,
                             Fee = price * 0.66M * 0.02M,
                             FeeAsset = "USDT",
                             Price = price/quantity,
                             Quantity = quantity*0.66M
                        },
                        new PurchaseTrade {
                            TradeId = 2,
                             Fee = price * 0.33M * 0.02M,
                             FeeAsset = "USDT",
                             Price = price/quantity,
                             Quantity = quantity*0.33M
                        }
                    }
            };
        }
      
    }
}
