using Crypto.Model;
using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;


namespace Crypto.BinanceControllers
{
    internal class DummyBinanceController :IBinanceController
    {
        #region "GetActualPrices"
        public async Task<List<ActualPrice>> GetActualPrices()
        {
            Random r = new Random();
            if (r.Next(5) != 0)
            {
                    return GetDummyPrices();
            }
            else
                throw new Exception("GET ACTUAL PRICE", new Exception("Dummy error message"));
        }
        public async Task<List<ActualPrice>> GetActualPrices(IEnumerable<string> symbols)
        {
            Random r = new Random();
            if (r.Next(5) != 0)
            {
                return GetDummyPrices().Where(x => symbols.ToList().Contains(x.Symbol)).ToList();
            }
            else
                throw new Exception("GET ACTUAL PRICE", new Exception("Dummy error message"));
        }
        private List<ActualPrice> GetDummyPrices()
        {
            return new List<ActualPrice>()
                {
                    new ActualPrice("ETHBTC", 0.07170700M),
                    new ActualPrice("LTCBTC", 0.00343100M),
                    new ActualPrice("GASBTC", 0.00012280M),
                    new ActualPrice("YFIBUSD",6162.00000000M),
                    new ActualPrice("BALUSDT",5.332M),
                    new ActualPrice("BNBBTC", 0.016106M),
                    new ActualPrice("NEOBTC", 0.000399M),
                    new ActualPrice("QTUMETI", 0.001706M),
                    new ActualPrice("OAXETH", 0.0001778M),
                    new ActualPrice("WTCBTC", 0.00001323M),
                };
        }
        #endregion
        #region "Buy"
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
        #endregion

       
      
    }
}
