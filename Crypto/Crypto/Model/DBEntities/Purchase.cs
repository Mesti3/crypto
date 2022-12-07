namespace Crypto.Model.Entities
{
    public class Purchase:Order
    {
        public DateTime? SaleDate { get; set; }
        private bool Sold => SaleDate.HasValue;
        public List<PurchaseTrade> Trades { get; set; }
    }
}
