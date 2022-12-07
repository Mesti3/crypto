using Crypto.Model.Entities;

namespace Crypto.Model.ServiceEntities
{
    public class OrderProfit:Order
    {
        private decimal? actualUnitPrice;
        public decimal? ActualUnitPrice 
        { 
            get=> actualUnitPrice;
            set {
                actualUnitPrice = value;
                if (value.HasValue)
                {                
                    TheoreticalPrice = value * Quantity;
                    Profit = TheoreticalPrice - TotalPrice;
                    ProfitRatio = TheoreticalPrice / TotalPrice  -1;
                    /*
                    TheoreticalPrice = ActualUnitPrice * Quantity;
                    Profit = TheoreticalPrice - Price;
                    ProfitRatio = TheoreticalPrice / Price;
                    */
                }
            }
           
        }

        public decimal? TheoreticalPrice {  get; private set; }
        public decimal? Profit { get; private set; }
        public decimal? ProfitRatio { get; private set; }
        /*
        public  decimal? TheoreticalPrice => !ActualUnitPrice.HasValue ? ActualUnitPrice * Quantity : null;
        public decimal? Profit => !ActualUnitPrice.HasValue ? ActualUnitPrice * Quantity - Price : null;
        public decimal? ProfitRatio => !ActualUnitPrice.HasValue ? (ActualUnitPrice * Quantity - Price) : null;
        */

        public OrderProfit() : base()
        {; }
        public OrderProfit(Order order) : base()
        {
            base.Quantity = order.Quantity;
            base.UnitPrice = order.UnitPrice;
            base.TotalPrice = order.TotalPrice;
            base.ClientOrderId = order.ClientOrderId;
            base.CreateTime = order.CreateTime;
            base.ExternalOrderId = order.ExternalOrderId;
            base.Id = order.Id;
            base.Symbol = order.Symbol;
        }
    }
}
