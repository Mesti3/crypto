namespace Crypto.Model.Entities
{
    public class Sale:Order
    {
        public List<SaleTrade> Trades { get; set; }
    }
}
