using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Crypto.Model;
using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;
using CryptoExchange.Net.Authentication;

namespace Crypto.BinanceControllers
{
    internal class TestBinanceController : IBinanceController
    {
        private ModelConverter Converter;
        public TestBinanceController(ModelConverter convertor, string apiKey, string apiSecret)
        {
            Converter = convertor;
            BinanceClient.SetDefaultOptions(new BinanceClientOptions() { ApiCredentials = new ApiCredentials(apiKey, apiSecret) });
        }

        #region "GetActualPrices"
        public async Task<ActualPrice> GetActualPrice(string symbol)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.ExchangeData.GetPriceAsync(symbol);
                if (result.Success)
                    return Converter.BinancePriceToActualPrice(result.Data);
                else
                    throw new Exception("GetActualPrice_TEST", new Exception(result.Error?.Message));
            }
        }

        public async Task<List<ActualPrice>> GetActualPrices()
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.ExchangeData.GetPricesAsync();
                if (result.Success)
                    return result.Data.Select(x => Converter.BinancePriceToActualPrice(x)).ToList();
                else
                    throw new Exception("GetActualPrice_TEST", new Exception(result.Error?.Message));
            }
        }

        public async Task<List<ActualPrice>> GetActualPrices(IEnumerable<string> symbols)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.ExchangeData.GetPricesAsync(symbols);
                if (result.Success)
                    return result.Data.Select(x => Converter.BinancePriceToActualPrice(x)).ToList();
                else
                    throw new Exception("GetActualPrice_TEST", new Exception(result.Error?.Message));
            }
        }
        #endregion
        #region "Buy"
        public async Task<Purchase> Buy(string symbol, decimal quantity, decimal price)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.Trading.PlaceTestOrderAsync(symbol, OrderSide.Buy, SpotOrderType.Limit, quantity: quantity, price: price, timeInForce: TimeInForce.GoodTillCanceled);//, newClientOrderId: ClientOrderId.GetNew(symbol)
                if (result.Success)
                    return Converter.BinancePlacedOrderToPurchase(result.Data);
                else
                    throw new Exception("Buy_TEST: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\", \"price\"" + price + "\"}", new Exception(result.Error?.Message));
            }
        }
        public async Task<Purchase> Buy(string symbol, decimal quantity)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.Trading.PlaceTestOrderAsync(symbol, OrderSide.Buy, SpotOrderType.Market, quantity: quantity);//, newClientOrderId: ClientOrderId.GetNew(symbol)
                if (result.Success)
                    return Converter.BinancePlacedOrderToPurchase(result.Data);
                else
                    throw new Exception("Buy_TEST: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\"}", new Exception(result.Error?.Message));
            }
        }
        #endregion
        #region "Sell"
        public async Task<Sale> Sell(string symbol, decimal quantity)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.Trading.PlaceTestOrderAsync(symbol, OrderSide.Sell, SpotOrderType.Market, quantity: quantity);//, newClientOrderId: ClientOrderId.GetNew(symbol)
                if (result.Success)
                    return Converter.BinancePlacedOrderToSale(result.Data);
                else
                    throw new Exception("Sell_TEST: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\"}", new Exception(result.Error?.Message));
            }
        }


        public Task<List<OrderProfit>> GetAllOpenOrders(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderProfit>> GetAllOpenOrders(string symbol, long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<object> DoSomething(string symbol, long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderProfit> GetOrder(string symbol, long orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderProfit>> GetOrders(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<SymbolSetting> GetSymbolSetting(string symbol)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}


