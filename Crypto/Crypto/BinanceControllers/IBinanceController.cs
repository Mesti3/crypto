using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;

namespace Crypto.BinanceControllers
{
    internal interface IBinanceController
    {
        #region "GetActualPrices"
        public Task<ActualPrice> GetActualPrice(string symbol);
        public Task<List<ActualPrice>> GetActualPrices();
        public Task<List<ActualPrice>> GetActualPrices(IEnumerable<string> symbols);
        #endregion
        #region "Buy"
        public Task<Purchase> Buy(string symbol, decimal quantity);
        public Task<Purchase> Buy(string symbol, decimal quantity, decimal price);
        #endregion
        #region "Sell"
        public Task<Sale> Sell(string symbol, decimal quantity);
        #endregion
        #region "GetOrders"
        public Task<OrderProfit> GetOrder(string symbol, long orderId);
        public Task<List<OrderProfit>> GetOrders(string symbol);
        #endregion
        #region "Settings"
        public Task<SymbolSetting> GetSymbolSetting(string symbol);
        #endregion
        #region "Account"
        public Task<Asset> GetWalletStatus();
        public Task<List<Asset>> GetAssets();
        #endregion
        #region "Other"
        public Task<object> DoSomething(string symbol, long orderId);
        #endregion

    }
}
