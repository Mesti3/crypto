namespace Crypto.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        /// <summary>
        /// id of order in binance system
        /// </summary>
        public long ExternalOrderId { get; set; }
        /// <summary>
        /// our orderId
        /// </summary>
        public string ClientOrderId { get; set; }
        /// <summary>
        /// unit price od sybol
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// price of order with fee included
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// number of sold/purhased units
        /// </summary>
        public decimal Quantity { get; set; }
        public DateTime CreateTime { get; set; }


    }
}
