using InventoryManagement_Backend.Models;

namespace InventoryManagement_Backend.Services
{
    public interface IInvoiceEmailService
    {
        Task SendOrderInvoiceEmailAsync(User? user, Supplier? supplier, List<PurchaseSalesOrders> orders, Transaction transaction, List<Product> products);
    }
}
