using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;

namespace InventoryManagement_Backend.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionCreateDto>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<Transaction?> UpdateAsync(int id, Transaction transaction);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Transaction>> FilterAsync(
            char? type,
            DateTime? date,
            int? customerId,
            int? supplierId);


      Task<(decimal Purchases, decimal Sales, decimal NoOfPurchases, decimal NoOfSales)> GetDailyReportAsync(DateTime date);
       Task<IEnumerable<object>> GetMonthlyReportAsync(int year, int month);
        Task<IEnumerable<object>> GetYearlyReportAsync(int year);

    }
}