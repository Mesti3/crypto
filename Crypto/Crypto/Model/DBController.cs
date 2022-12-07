
using Crypto.Model.Entities;

namespace Crypto.Model
{
    internal class DBController
    {
        public readonly DataContext Context;
        public DBController(string connectionString)
        {
            Context = new DataContext(connectionString);
        }
        public void SavePurchase(Purchase newPurchase)
        {
            Context.Purchase.Add(newPurchase);
            Context.SaveChanges();
        }
        public void SaveSale (Sale newSale)
        {
            Context.Sale.Add(newSale);
            Context.SaveChanges();
        }
        public List<Purchase> LoadPurchases()
        {
            return Context.Purchase.Where(x=>x.SaleDate == null).ToList();
            
        }
    }
}
