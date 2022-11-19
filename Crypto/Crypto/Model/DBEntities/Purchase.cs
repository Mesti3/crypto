namespace Crypto.Model.Entities
{
    public class Purchase:Order
    {
        public List<PurchaseTrade> Trades { get; set; }
    }
}
