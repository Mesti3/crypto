namespace Crypto.Model.Entities
{
    public class SaleTrade:Trade
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
