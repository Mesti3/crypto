namespace Crypto.Model.Entities
{
    public class PurchaseTrade:Trade
    {
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
    }
}
