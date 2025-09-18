using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;

namespace InventoryManagement_Backend.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllAsync();
        Task<TransactionDto?> GetByIdAsync(int id);
        Task<TransactionDto> CreateAsync(TransactionCreateDto transaction);
        Task<TransactionDto?> UpdateAsync(int id, TransactionCreateDto transaction);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TransactionDto>> FilterAsync(
            string? type,
            DateTime? date, int? productId, int? supplierId, int? customerId);
    }
}