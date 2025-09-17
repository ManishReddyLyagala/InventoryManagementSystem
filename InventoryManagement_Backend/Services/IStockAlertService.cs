using InventoryManagement_Backend.Models;

namespace InventoryManagement_Backend.Services
{
    public interface IStockAlertService
    {
        Task<List<Product>> GetLowStockProductsAsync(int threshold);
        Task SendLowStockEmailAsync(List<Product> lowStockProducts);
        Task SendDailyLowStockEmailAsync(int threshold);
    }
}
