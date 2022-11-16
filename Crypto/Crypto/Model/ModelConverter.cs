using Binance.Net.Objects.Models.Spot;
using Crypto.Model.Entities;

namespace Crypto.Model
{
    internal class ModelConverter
    {
        public Purchase BinancePlacedOrderToPurchase(BinancePlacedOrder placedOrder)
        {
            Purchase purchase = new Purchase();
            purchase.Symbol = placedOrder.Symbol;
            purchase.Price = placedOrder.Price;
            purchase.Quantity = placedOrder.Quantity;
            purchase.ClientOrderId = placedOrder.ClientOrderId;
            purchase.CreateTime = placedOrder.CreateTime;
            purchase.Trades = placedOrder.Trades.Select(x => BinanceTradeToPurchaseTrade(x)).ToList();
            return purchase;
        }

        private PurchaseTrade BinanceTradeToPurchaseTrade(BinanceOrderTrade binanceOrderTrade)
        {
            PurchaseTrade purchaseTrade = new PurchaseTrade();
            purchaseTrade.TradeId = binanceOrderTrade.Id;
            purchaseTrade.Price = binanceOrderTrade.Price;
            purchaseTrade.Quantity = binanceOrderTrade.Quantity;
            purchaseTrade.Fee = binanceOrderTrade.Fee;
            purchaseTrade.FeeAsset = binanceOrderTrade.FeeAsset;
            return purchaseTrade;
        }
    }
}