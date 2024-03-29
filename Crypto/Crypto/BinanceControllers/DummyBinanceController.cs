﻿using Crypto.Model;
using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;


namespace Crypto.BinanceControllers
{
    internal class DummyBinanceController : IBinanceController
    {
        #region "GetActualPrices"
        public async Task<ActualPrice> GetActualPrice(string symbol)
        {
            Random r = new Random();
            if (r.Next(5) != 0)
            {
                return GetDummyPrices().Where(x => x.Symbol == symbol).FirstOrDefault();
            }
            else
                throw new Exception("GetActualPrice_Dummy", new Exception("Dummy error message"));
        }
        public async Task<List<ActualPrice>> GetActualPrices()
        {
            Random r = new Random();
            if (r.Next(5) != 0)
            {
                return GetDummyPrices();
            }
            else
                throw new Exception("GetActualPrices_Dummy", new Exception("Dummy error message"));
        }
        public async Task<List<ActualPrice>> GetActualPrices(IEnumerable<string> symbols)
        {
            Random r = new Random();
            if (r.Next(5) != 0)
            {
                return GetDummyPrices().Where(x => symbols.ToList().Contains(x.Symbol)).ToList();
            }
            else
                throw new Exception("GetActualPrices_Dummy", new Exception("Dummy error message"));
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
                return GetDummyPurchase(symbol, quantity, price);
            else
                throw new Exception("Buy_Dummy: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\", \"price\"" + price + "\"}", new Exception("Dummy error message"));

        }
        public async Task<Purchase> Buy(string symbol, decimal quantity)
        {
            Random r = new Random();
            if (r.Next(5) != 0)
            {
                var actualPrice = GetDummyPrices().Where(x => x.Symbol == symbol).FirstOrDefault()?.Price ?? r.Next(1000, 1000000) * 0.01M;
                return GetDummyPurchase(symbol, quantity, actualPrice * quantity);
            }
            else
                throw new Exception("Buy_Dummy: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\"}", new Exception("Dummy error message"));

        }

        private Purchase GetDummyPurchase(string symbol, decimal quantity, decimal price)
        {
            return new Purchase()
            {
                ClientOrderId = ClientOrderId.GetNew(symbol),
                Symbol = symbol,
                CreateTime = DateTime.Now,
                TotalPrice = price,
                UnitPrice = price = price / quantity,
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

        #region "Sell"
        public async Task<Sale> Sell(string symbol, decimal quantity)
        {
            Random r = new Random();
            if (r.Next(5) != 0)
            {
                return GetDummySale(symbol, quantity, r.Next(1000, 1000000) * 0.01M);
            }
            else
                throw new Exception("Buy_Dummy: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\"}", new Exception("Dummy error message"));

        }

        private Sale GetDummySale(string symbol, decimal quantity, decimal price)
        {
            return new Sale()
            {
                ClientOrderId = ClientOrderId.GetNew(symbol),
                Symbol = symbol,
                CreateTime = DateTime.Now,
                TotalPrice = price,
                UnitPrice = price = price / quantity,
                Quantity = quantity,
                Trades = new List<SaleTrade>() {
                        new SaleTrade {
                            TradeId = 1,
                             Fee = price * 0.66M * 0.02M,
                             FeeAsset = "USDT",
                             Price = price/quantity,
                             Quantity = quantity*0.66M
                        },
                        new SaleTrade {
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


        #region "GetOrders"
        public async Task<OrderProfit> GetOrder(string symbol, long orderId)
        {
            return new OrderProfit()
            {
                ExternalOrderId = orderId,
                Symbol = symbol,
                Quantity = 5,
                ActualUnitPrice = 1.02M,
                ClientOrderId = "test0",
                CreateTime = new DateTime(2022, 01, 01),
                Id = 23,
                TotalPrice = 5.1M,
                UnitPrice = 1.025M
            };
        }
        public async Task<List<OrderProfit>> GetOrders(string symbol)
        {
            return new List<OrderProfit>
            {
                new OrderProfit()
                {
                    ExternalOrderId = 12548,
                    Symbol = symbol,
                    Quantity = 5,
                    ActualUnitPrice = 1.02M,
                    ClientOrderId = "test0",
                    CreateTime = new DateTime(2022, 01, 01),
                    Id = 23,
                    TotalPrice = 5.1M,
                    UnitPrice = 1.025M
                },
                new OrderProfit()
                {
                    ExternalOrderId = 24821,
                    Symbol = symbol,
                    Quantity = 2,
                    ActualUnitPrice = 1.01M,
                    ClientOrderId = "test0",
                    CreateTime = new DateTime(2022, 03, 01),
                    Id = 24,
                    TotalPrice = 2.02M,
                    UnitPrice = 1.025M
                }
            };
        }
        #endregion
        #region "Settings"
        public async Task<SymbolSetting> GetSymbolSetting(string symbol)
        {
            return new SymbolSetting()
            {
                Symbol = symbol,
                MinPrice = 1,
                MaxPrice = 1000000,
                PriceStep = 0.01M,
                MinQuantity = 0.01M,
                MaxQuantity = 1000000,
                QuantityStep = 0.001M
            };
        }
        #endregion
        #region "Account"
        public async Task<Asset> GetWalletStatus()
        {
            return new Asset()
            {
                Symbol = "EUR",
                Available = 18.25M,
                Total = 18.25M
            };
        }
        public async Task<List<Asset>> GetAssets()
        {
            return new List<Asset>()
            {
                new Asset()
                {
                    Symbol = "EUR",
                    Available = 18.25M,
                    Total = 18.25M
                },
                new Asset()
                {
                    Symbol = "BTC",
                    Available = 0.00125M,
                    Total = 0.0125M
                }
            };
        }
        #endregion
        #region "Other"
        public async Task<object> DoSomething(string symbol, long orderId)
        {

                throw new NotImplementedException();
            
        }
        #endregion
    }
}
