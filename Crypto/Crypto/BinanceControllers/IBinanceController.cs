using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;

namespace Crypto.BinanceControllers
{
    internal interface IBinanceController
    {
        public Task<ActualPrice> GetActualPrice(string symbol);
        public Task<List<ActualPrice>> GetActualPrices();
        public Task<List<ActualPrice>> GetActualPrices(IEnumerable<string> symbols);
        public Task<Purchase> Buy(string symbol, decimal quantity);
        public Task<Purchase> Buy(string symbol, decimal quantity, decimal price);
        public Task<Sale> Sell(string symbol, decimal quantity);
        public Task<OrderProfit> GetOrder(string symbol, long orderId);
        public Task<List<OrderProfit>> GetOrders(string symbol);

        public Task<object> DoSomething(string symbol, long orderId);
        public Task<SymbolSetting> GetSymbolSetting(string symbol);
    }
}
