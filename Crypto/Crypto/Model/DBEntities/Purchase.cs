namespace Crypto.Model.Entities
{
    internal class Purchase
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string ClientOrderId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreateTime { get; set; }

        public List<PurchaseTrade> Trades { get; set; }
    }
}
