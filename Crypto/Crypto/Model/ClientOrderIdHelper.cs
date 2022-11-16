namespace Crypto.Model
{
    internal class ClientOrderId
    {
        public static string GetNew(string symbol) => symbol.Trim() + DateTime.Now.Ticks;

    }
}
