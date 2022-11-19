using Binance.Net.Clients;
using Binance.Net.Enums;
using Crypto.Model;
using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;

namespace Crypto.BinanceControllers
{
    internal class BinanceController : IBinanceController
    {
        private ModelConverter Converter;
        public BinanceController(ModelConverter convertor)
        {
            Converter = convertor;
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
                    throw new Exception("GetActualPrice", new Exception(result.Error?.Message));
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
                    throw new Exception("GetActualPrice", new Exception(result.Error?.Message));
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
                    throw new Exception("GetActualPrice", new Exception(result.Error?.Message));
            }
        }

        #endregion
        #region "Buy"
        public async Task<Purchase> Buy(string symbol, decimal quantity, decimal price)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.Trading.PlaceOrderAsync(symbol, OrderSide.Buy, SpotOrderType.Limit, quantity: quantity, price: price, timeInForce: TimeInForce.GoodTillCanceled);
                if (result.Success)
                    return Converter.BinancePlacedOrderToPurchase(result.Data);
                else
                    throw new Exception("Buy: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\", \"price\"" + price + "\"}", new Exception(result.Error?.Message));
            }
        }
        public async Task<Purchase> Buy(string symbol, decimal quantity)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.Trading.PlaceOrderAsync(symbol, OrderSide.Buy, SpotOrderType.Market, quantity: quantity);
                if (result.Success)
                    return Converter.BinancePlacedOrderToPurchase(result.Data);
                else
                    throw new Exception("Buy: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\"}", new Exception(result.Error?.Message));
            }
        }
        #endregion
        #region "Sell"
        public async Task<Sale> Sell(string symbol, decimal quantity)
        {
            using (var client = new BinanceClient())
            {
                var result = await client.SpotApi.Trading.PlaceOrderAsync(symbol, OrderSide.Sell, SpotOrderType.Market, quantity: quantity);
                if (result.Success)
                    return Converter.BinancePlacedOrderToSale(result.Data);
                else
                    throw new Exception("Sell: {\"symbol\":\"" + symbol + "\", \"quantity\":\"" + quantity + "\"}", new Exception(result.Error?.Message));
            }
        }
        #endregion
    }
}
