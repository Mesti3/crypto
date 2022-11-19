using Crypto.BinanceControllers;
using Crypto.Model;
using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;
using Serilog;

namespace Crypto
{
    public class TradeController
    {

        private IBinanceController BinanceController;
        private DBController DBController;
        private ILogger Logger;
        private string ErrorFormatString = "{Timestamp} [{Level}] {Message}{NewLine}{Exception}";

        public TradeController()
        {
            Initialize();
        }

        private void Initialize()
        {
            var Config = new ConfigReader();
            BinanceController = Config.GetBinanceReaderFromConfig();
            DBController = Config.GetDBControllerFromConfig();
            Logger = Config.GetLoggerFromConfig();
        }

        #region "GetActualPrices"
        /// <summary>
        /// load list of all symbols with actual price
        /// </summary>
        /// <returns>loaded prices</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#symbol-price-ticker"/>
        public async Task<List<ActualPrice>> GetActualPrices()
        {
            try
            {
                return await BinanceController.GetActualPrices();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }
        }

        /// <summary>
        /// get actual price of given symbol in list
        /// </summary>
        /// <returns>loaded prices</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#symbol-price-ticker"/>
        public async Task<List<ActualPrice>> GetActualPrices(string symbol)
        {
            try
            {
                return await BinanceController.GetActualPrices(new string[1] { symbol });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }
        }

        /// <summary>
        /// get actual price of given symbols 
        /// </summary>
        /// <returns>loaded prices</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#symbol-price-ticker"/>
        public async Task<List<ActualPrice>> GetActualPrices(IEnumerable<string> symbols)
        {
            try
            {
                return await BinanceController.GetActualPrices( symbols );
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// buy symbol in given quantity or price
        /// </summary>
        /// <param name="symbol">code of good to buy</param>
        /// <param name="price">price</param>
        /// <param name="quantity">number of units</param>
        /// <remarks>either price or quantity must be filled</remarks>
        /// <returns>created order</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#new-order-trade"/>
        public async Task<bool> BuyAsync(string symbol, decimal quantity, decimal? price = null)
        {

            //no validation - parameters are vaidated in BinanceNet library, PlaceOrderAsync call
            try
            {
                Purchase createdPurchase;
                if (price.HasValue && price.Value > 0)
                    createdPurchase = await BinanceController.Buy(symbol, quantity, price.Value);
                else
                    createdPurchase = await BinanceController.Buy(symbol, quantity);
                DBController.SavePurchase(createdPurchase);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                return false;
            }

        }

    }

}

