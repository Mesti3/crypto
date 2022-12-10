using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Crypto.Model;
using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;
using CryptoExchange.Net.Authentication;

namespace Crypto.BinanceControllers
{
    internal class BinanceController : IBinanceController
    {
        private ModelConverter Converter;
        public BinanceController(ModelConverter convertor, string apiKey, string apiSecret)
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

        #region "GetOrders"
        public async Task<OrderProfit> GetOrder(string symbol, long orderId)
        {
            using (var client = new BinanceClient(BinanceClientOptions.Default))
            {
                var result = await client.SpotApi.Trading.GetOrdersAsync(symbol, orderId);
                if (result.Success)
                    if (result.Data.Any())
                        return Converter.BinanceOrderToOrderProfit(result.Data.First());
                    else
                        throw new Exception("GetAllOpenOrders: {\"symbol\":\"" + symbol + "\", \"orderId\":\"" + orderId + "\"}", new Exception("Order not found"));
                else
                    throw new Exception("GetAllOpenOrders: {\"symbol\":\"" + symbol + "\", \"orderId\":\"" + orderId + "\"}", new Exception(result.Error?.Message));
            }
        }
        public async Task<List<OrderProfit>> GetOrders(string symbol)
        {
            using (var client = new BinanceClient(BinanceClientOptions.Default))
            {
                var result = await client.SpotApi.Trading.GetOrdersAsync(symbol);
                if (result.Success)
                    return result.Data.Where(x => x.Side == OrderSide.Buy).Select(x => Converter.BinanceOrderToOrderProfit(x)).ToList();
                else
                    throw new Exception("GetAllOpenOrders: {\"symbol\":\"" + symbol + "\"}", new Exception(result.Error?.Message));
            }
        }
        #endregion
        #region "Settings"
        public async Task<SymbolSetting> GetSymbolSetting(string symbol)
        {
            using (var client = new BinanceClient(BinanceClientOptions.Default))
            {
                var result = await client.SpotApi.ExchangeData.GetExchangeInfoAsync(symbol);
                if (result.Success)
                    if (result.Data.Symbols.Any())
                        return Converter.BinanceSymbolToSymbolSetting(result.Data.Symbols.First());
                    else
                        throw new Exception("GetSymbolSetting: {\"symbol\":\"" + symbol + "\"}", new Exception("Symbol setting not found"));
                else
                    throw new Exception("GetSymbolSetting: {\"symbol\":\"" + symbol + "\"}", new Exception(result.Error?.Message));
            }
        }
        #endregion
        #region "Account"
        public async Task<Asset> GetWalletStatus()
        {
            using (var client = new BinanceClient(BinanceClientOptions.Default))
            {
                var result = await client.SpotApi.Account.GetBalancesAsync("EUR");
                if (result.Success)
                    return Converter.BinanceUserBalanceToAsset(result.Data.First());
                else
                    throw new Exception("GetWalletStatus: {}", new Exception(result.Error?.Message));
            }
        }
        public async Task<List<Asset>> GetAssets()
        {
            using (var client = new BinanceClient(BinanceClientOptions.Default))
            {
                var result = await client.SpotApi.Account.GetBalancesAsync();
                if (result.Success)
                    return result.Data.Select(x => Converter.BinanceUserBalanceToAsset(x)).ToList();
                else
                    throw new Exception("GetAssets: {}", new Exception(result.Error?.Message));
            }
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