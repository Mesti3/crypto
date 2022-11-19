using Binance.Net.Objects.Models.Spot;
using Crypto.Model.Entities;
using Crypto.Model.ServiceEntities;

namespace Crypto.Model
{
    internal class ModelConverter
    {
        public ActualPrice BinancePriceToActualPrice(BinancePrice price)
        {
            return new ActualPrice(price.Symbol, price.Price);
        }
        public Order BinancePlacedOrderToOrder(BinancePlacedOrder placedOrder)
        {
            Order order = new Order();
            order.Symbol = placedOrder.Symbol;
            order.Price = placedOrder.Price;
            order.Quantity = placedOrder.Quantity;
            order.ClientOrderId = placedOrder.ClientOrderId;
            order.CreateTime = placedOrder.CreateTime;
            return order;
        }

        public Purchase BinancePlacedOrderToPurchase(BinancePlacedOrder placedOrder)
        {
            Purchase purchase = (Purchase)BinancePlacedOrderToOrder(placedOrder);
            purchase.Trades = placedOrder.Trades.Select(x => BinanceTradeToPurchaseTrade(x)).ToList();
            return purchase;
        }
        public Sale BinancePlacedOrderToSale(BinancePlacedOrder placedOrder)
        {
            Sale sale = (Sale)BinancePlacedOrderToOrder(placedOrder);
            sale.Trades = placedOrder.Trades.Select(x => BinanceTradeToSaleTrade(x)).ToList();
            return sale;
        }
        private Trade BinanceTradeToTrade(BinanceOrderTrade binanceOrderTrade)
        {
            Trade trade = new Trade();
            trade.TradeId = binanceOrderTrade.Id;
            trade.Price = binanceOrderTrade.Price;
            trade.Quantity = binanceOrderTrade.Quantity;
            trade.Fee = binanceOrderTrade.Fee;
            trade.FeeAsset = binanceOrderTrade.FeeAsset;
            return trade;
        }
        private PurchaseTrade BinanceTradeToPurchaseTrade(BinanceOrderTrade binanceOrderTrade)
        {
            PurchaseTrade purchaseTrade = (PurchaseTrade) BinanceTradeToTrade(binanceOrderTrade);
            return purchaseTrade;
        }
        private SaleTrade BinanceTradeToSaleTrade(BinanceOrderTrade binanceOrderTrade)
        {
            SaleTrade saleTrade = (SaleTrade)BinanceTradeToTrade(binanceOrderTrade);
            return saleTrade;
        }
    }
}