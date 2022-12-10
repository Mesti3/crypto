using Binance.Net.Objects.Models;
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
        public void FillBinancePlacedOrderToOrder(BinanceOrderBase placedOrder, Order order)
        {

            order.Symbol = placedOrder.Symbol;
            order.UnitPrice = placedOrder.Price;
            order.Quantity = placedOrder.Quantity;
            order.ClientOrderId = placedOrder.ClientOrderId;
            order.ExternalOrderId = placedOrder.Id;
            order.CreateTime = placedOrder.CreateTime;
        
        }

        public Purchase BinancePlacedOrderToPurchase(BinancePlacedOrder placedOrder)
        {
            Purchase purchase = new Purchase();
            FillBinancePlacedOrderToOrder(placedOrder, purchase);
            if (placedOrder.Trades != null)
            {
                purchase.Trades = placedOrder.Trades.Select(x => BinanceTradeToPurchaseTrade(x)).ToList();
                purchase.TotalPrice = GetTotalPrice(purchase.Trades.Select(x=>(Trade)x), purchase.UnitPrice, purchase.Quantity);
            }
            purchase.TotalPrice = GetTotalPrice(purchase.Trades.Select(x => (Trade)x), purchase.UnitPrice, purchase.Quantity);
            return purchase;
        }

        private decimal GetTotalPrice(IEnumerable<Trade>? trades, decimal unitPrice, decimal quantity )
        {
            if (trades != null && trades.Count() > 0)
                return trades.Sum(x => x.Price * x.Quantity + x.Fee);
            else
                return unitPrice * quantity; 
        }

        public Sale BinancePlacedOrderToSale(BinancePlacedOrder placedOrder)
        {
            Sale sale = new Sale();
            FillBinancePlacedOrderToOrder(placedOrder, sale);
            sale.Trades = placedOrder.Trades.Select(x => BinanceTradeToSaleTrade(x)).ToList();
            sale.TotalPrice = GetTotalPrice(sale.Trades.Select(x => (Trade)x), sale.UnitPrice, sale.Quantity);
            return sale;
        }
        private void FillBinanceTradeToTrade(BinanceOrderTrade binanceOrderTrade, Trade trade)
        {
            trade.TradeId = binanceOrderTrade.Id;
            trade.Price = binanceOrderTrade.Price;
            trade.Quantity = binanceOrderTrade.Quantity;
            trade.Fee = binanceOrderTrade.Fee;
            trade.FeeAsset = binanceOrderTrade.FeeAsset;
   
        }
        private PurchaseTrade BinanceTradeToPurchaseTrade(BinanceOrderTrade binanceOrderTrade)
        {
            PurchaseTrade purchaseTrade = new PurchaseTrade();
            FillBinanceTradeToTrade(binanceOrderTrade, purchaseTrade);
            return purchaseTrade;
        }
        private SaleTrade BinanceTradeToSaleTrade(BinanceOrderTrade binanceOrderTrade)
        {
            SaleTrade saleTrade = new SaleTrade();
            FillBinanceTradeToTrade(binanceOrderTrade, saleTrade);
            return saleTrade;
        }

        internal OrderProfit BinanceOrderToOrderProfit(BinanceOrder data)
        {
            OrderProfit order = new OrderProfit();
            FillBinancePlacedOrderToOrder(data,order);

            order.TotalPrice = GetTotalPrice(null, order.UnitPrice, order.Quantity);
            return order;
        }
        internal SymbolSetting BinanceSymbolToSymbolSetting(BinanceSymbol data)
        {
            return new SymbolSetting()
            {
                Symbol = data.Name,
                UnitPricePrecision = data.BaseAssetPrecision,
                MinPrice = data.PriceFilter?.MinPrice ?? 10,
                MaxPrice = data.PriceFilter?.MaxPrice ?? 10000, 
                PriceStep = data.PriceFilter?.TickSize ?? 0.01M,
                MinQuantity = data.LotSizeFilter?.MinQuantity ?? 0.001M,
                MaxQuantity = data.LotSizeFilter?.MaxQuantity ?? 10000000,
                QuantityStep = data.LotSizeFilter?.StepSize ?? 0.00001M,

            };
        }

        internal Asset BinanceUserBalanceToAsset(BinanceUserBalance data)
        {
            return new Asset()
            {
                Symbol = data.Asset,
                Available = data.Available, 
                Total = data.Total
            };
        }
    }
}