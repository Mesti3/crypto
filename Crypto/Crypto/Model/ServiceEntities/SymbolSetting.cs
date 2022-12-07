namespace Crypto.Model.ServiceEntities
{
    public  class SymbolSetting
    {
        public string Symbol { get; set; }
        public decimal UnitPricePrecision { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal PriceStep { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxQuantity { get; set; }
        public decimal QuantityStep { get; set; }

        public decimal GetValidPrice(decimal price)
        {
            if (price > MaxPrice)
                return MaxPrice; //we can place more orders, but now rather not
            if (price < MinPrice)
                throw new ArgumentOutOfRangeException(nameof(price), "minimal valid price: " + MinPrice);
            var ratio = (price - MinPrice) / PriceStep;
            if (ratio == (int)ratio)
                return price;
            else
                return (int)ratio * PriceStep + MinPrice;
        }
        public decimal GetValidQuantity(decimal quantity)
        {
            if (quantity > MaxQuantity)
                return MaxQuantity; //we can place more orders, but now rather not
            if (quantity < MinQuantity)
                throw new ArgumentOutOfRangeException(nameof(quantity), "minimal valid quantity: " + MinQuantity);
            var ratio = (quantity - MinQuantity) / QuantityStep;
            if (ratio == (int)ratio)
                return quantity;
            else
                return (int)ratio * QuantityStep + MinQuantity; 
        }
    }
}
