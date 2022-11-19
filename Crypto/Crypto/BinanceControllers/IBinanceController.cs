using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;

namespace Crypto.BinanceControllers
{
    internal interface IBinanceController
    {
        public Task<List<ActualPrice>> GetActualPrices();
        public Task<List<ActualPrice>> GetActualPrices(IEnumerable<string> symbols);
        public Task<Purchase> Buy(string symbol, decimal quantity);
        public Task<Purchase> Buy(string symbol, decimal quantity, decimal price);
    }
}
