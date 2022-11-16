
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
    }
}
