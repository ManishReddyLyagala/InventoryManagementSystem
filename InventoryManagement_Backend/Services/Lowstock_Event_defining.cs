using InventoryManagement_Backend.Models;

namespace InventoryManagement_Backend.Services
{
    public class Lowstock_Event_defining:EventArgs
    {
        public Product Product { get; set; }
        public Lowstock_Event_defining(Product product)
        {
            this.Product = product;
        }
    }
}
