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
        private Dictionary<string, SymbolSetting> SymbolSettings;

        public TradeController()
        {
            Initialize();
        }

        private void Initialize()
        {
            var Config = new ConfigReader();
            BinanceController = Config.GetBinanceControllerFromConfig();
            DBController = Config.GetDBControllerFromConfig();
            Logger = Config.GetLoggerFromConfig();
            SymbolSettings = new Dictionary<string, SymbolSetting>();
        }

        #region "GetSymbolsSetting"
        /// <summary>
        /// get actual price of given symbol in list
        /// </summary>
        /// <returns>loaded prices</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#exchange-information"/>
        public async Task<SymbolSetting> GetSymbolSetting(string symbol)
        {
            try
            {
                return await BinanceController.GetSymbolSetting(symbol);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }
        }
        #endregion
        #region "GetActualPrices"
        /// <summary>
        /// get actual price of given symbol in list
        /// </summary>
        /// <returns>loaded prices</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#symbol-price-ticker"/>
        public async Task<ActualPrice> GetActualPrice(string symbol)
        {
            try
            {
                return await BinanceController.GetActualPrice(symbol);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }
        }
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
        #region "Buy"
        /// <summary>
        /// buy symbol in given quantity or price
        /// </summary>
        /// <param name="symbol">code of good to buy</param>
        /// <param name="price">total price</param>
        /// <param name="quantity">number of units, up to 6 decimal places</param>
        /// <remarks>either price or quantity must be filled</remarks>
        /// <returns>created order</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#new-order-trade"/>
        public async Task<Purchase> BuyAsync(string symbol, decimal? quantity, decimal? price = null)
        {

            //no symbol validation - parameters are vaidated in BinanceNet library, PlaceOrderAsync call
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentNullException(nameof(symbol));
            if (!quantity.HasValue && !price.HasValue)
                throw new ArgumentNullException("Quantity or price must be filled");
            if (quantity.HasValue && price.HasValue)
                throw new ArgumentException("Either quantity or price must be filled");
            if (quantity.HasValue && quantity.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be more then 0");
            if (price.HasValue && price.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(price),"Price must be more then 0");
            try
            {
                Purchase createdPurchase;
                if (!quantity.HasValue)
                {
                    quantity = await CalculateQuantity(symbol, price!.Value);
                }
                var setting = await LoadSymbolSetting(symbol);
                quantity = setting.GetValidQuantity(quantity.Value);
                //if (price.HasValue && price.Value > 0) 
                //    createdPurchase = await BinanceController.Buy(symbol, quantity.Value, price.Value); //buy for current amount and price is not supported
                //else
                createdPurchase = await BinanceController.Buy(symbol,quantity.Value);
                DBController.SavePurchase(createdPurchase);
                return createdPurchase;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }

        }

        private async Task<decimal> CalculateQuantity(string symbol, decimal totalPrice)
        {
            var actualPrice = await GetActualPrice(symbol);
            if (actualPrice == null)
                throw new ArgumentOutOfRangeException(nameof(symbol));
            else if (actualPrice.Price == 0)
                throw new ArgumentOutOfRangeException(string.Format("{0} - Actual unit value for {1} is 0", nameof(symbol), symbol));
            else            
                return  totalPrice / actualPrice.Price;
            
        }
        private async Task<SymbolSetting> LoadSymbolSetting(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentNullException(nameof(symbol));
            if(SymbolSettings.ContainsKey(symbol))
                return SymbolSettings[symbol];
            else
            {
                var setting = GetSymbolSetting(symbol).Result;
                if (setting != null)
                {
                    SymbolSettings.Add(symbol, setting);
                    return setting;
                }
                else
                    throw new ArgumentNullException(nameof(symbol), "setting for " + symbol + " not found");
            }
        }

        #endregion
        #region "Sale"
        /// <summary>
        /// sell symbol in given quantity or price
        /// </summary>
        /// <param name="symbol">code of good to buy</param>
        /// <param name="quantity">number of units</param>
        /// <remarks>either price or quantity must be filled</remarks>
        /// <returns>created order</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#new-order-trade"/>
        public async Task<Sale> SellAsync(string symbol, decimal? quantity, decimal? price = null)
        {

            //no symbol validation - parameters are vaidated in BinanceNet library, PlaceOrderAsync call
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentNullException(nameof(symbol));
            if (!quantity.HasValue && !price.HasValue)
                throw new ArgumentNullException("Quantity or price must be filled");
            if (quantity.HasValue && price.HasValue)
                throw new ArgumentException("Either quantity or price must be filled");
            if (quantity.HasValue && quantity.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be more then 0");
            if (price.HasValue && price.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be more then 0");
            try
            {
                Sale createdSale;
                if (!quantity.HasValue)
                {
                    quantity = await CalculateQuantity(symbol, price!.Value);
                }
                
                createdSale = await BinanceController.Sell(symbol, quantity.Value);
                DBController.SaveSale(createdSale);
                return createdSale;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }

        }

        #endregion

        #region "Profit"
        /// <summary>
        /// sell symbol in given quantity or price
        /// </summary>
        /// <param name="symbol">code of good to buy</param>
        /// <remark>profit info is calculated additionally by us</remark>
        /// <returns>created order</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#curent-open-orders-user_data"/>
        public async Task<List<OrderProfit>> GetAllOrdersProfit(string symbol)
        {                    
            try
            {
                if (string.IsNullOrWhiteSpace(symbol))
                    throw new ArgumentNullException(nameof(symbol));
                var createdSale = await BinanceController.GetOrders(symbol);
                var price = await GetActualPrice(symbol);
                createdSale.ForEach(x => x.ActualUnitPrice = price.Price);
                return createdSale;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }

        }
        /// <summary>
        /// sell symbol in given quantity or price
        /// </summary>
        /// <param name="symbol">code of good to buy</param>
        /// <remark>profit info is calculated additionally by us</remark>
        /// <returns>created order</returns>
        /// <see cref="https://binance-docs.github.io/apidocs/spot/en/#curent-open-orders-user_data"/>
        public async Task<List<OrderProfit>> GetDBOrdersProfit()
        {
            try
            {
                var purchases = DBController.LoadPurchases().Select(x=>new OrderProfit(x)).ToList();
                var actualPrices = await GetActualPrices(purchases.Select(x => x.Symbol).Distinct());

                purchases.ForEach(x => x.ActualUnitPrice = actualPrices.FirstOrDefault(y => y.Symbol == x.Symbol)?.Price);
                return purchases;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }

        }

        #endregion

        #region "test"
        public async void DoSomethingAsync(string symbol, decimal? currentUnitPrice = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(symbol))
                    throw new ArgumentNullException(nameof(symbol));
                var createdSale = await BinanceController.DoSomething(symbol,0);
               
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ErrorFormatString);
                throw;
            }

        }
        #endregion
    }

}

