namespace Crypto.Model.ServiceEntities
{
    public class ActualPrice
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }

        public ActualPrice()
        { }

        public ActualPrice(string symbol, decimal price)
        {
            Symbol = symbol;
            Price = price;
        }

        public override string ToString()
        {
            return Symbol + ", " + Price;
        }
    }
}
