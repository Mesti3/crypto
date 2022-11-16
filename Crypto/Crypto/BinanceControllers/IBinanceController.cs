using Crypto.Model.Entities;

namespace Crypto.BinanceControllers
{
    internal interface IBinanceController
    {
        public Task<Purchase> Buy(string symbol, decimal quantity);
        public Task<Purchase> Buy(string symbol, decimal quantity, decimal price);
    }
}
