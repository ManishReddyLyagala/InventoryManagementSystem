using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using Microsoft.EntityFrameworkCore;
namespace InventoryManagement_Backend.Services
{
    public interface IPurchaseSalesOrdersService
    {
        Task<IEnumerable<Models.PurchaseSalesOrders>> GetAllAsync();
        Task<Models.PurchaseSalesOrders?> GetByIdAsync(int id);
        Task<IEnumerable<Models.PurchaseSalesOrders>> GetByTypeAsync(string type);
        Task<List<PurchaseSalesOrders>> CreateOrderAsync(OrderCreateRequest order);
        Task<Models.PurchaseSalesOrders?> UpdateAsync(int id, PurchaseSalesOrderDto order);
        Task<bool> DeleteAsync(int id);

        // Analytics
        Task<decimal> GetTotalSalesAsync();
        Task<decimal> GetTotalPurchasesAsync();
        Task<decimal> GetProfitAsync();
        Task<IEnumerable<object>> GetMonthlyTrendsAsync(int year);
        Task<IEnumerable<object>> GetYearlyTrendsAsync();

        Task<IEnumerable<object>> GetWeeklyTrendsAsync();
        Task<IEnumerable<object>> GetTodayTrendsAsync();
        Task<IEnumerable<object>> GetCustomTrendsAsync(DateTime startDate, DateTime endDate);
    }
}
