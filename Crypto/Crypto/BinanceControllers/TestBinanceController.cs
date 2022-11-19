using Binance.Net.Clients;
using Binance.Net.Enums;
using Crypto.Model;
using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;

namespace Crypto.BinanceControllers
{
    internal class TestBinanceController : IBinanceController
    {
        private ModelConverter Converter;
        public TestBinanceController(ModelConverter convertor)
        {
            Converter = convertor;
        }

        #region "GetActualPrices"
        public async Task<List<ActualPrice>> GetActualPrices()
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.ExchangeData.GetPricesAsync();
                if (result.Success)
                    return result.Data.Select(x=>Converter.BinancePriceToActualPrice(x)).ToList();
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
    }
}


