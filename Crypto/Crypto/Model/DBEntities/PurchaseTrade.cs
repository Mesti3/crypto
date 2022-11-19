namespace Crypto.Model.Entities
{
    internal class PurchaseTrade
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Fee { get; set; }
        public string FeeAsset { get; set; }
        public long TradeId { get; set; }
        public int PurchaseId { get; set; }

        public Purchase Purchase { get; set; }
    }
}
