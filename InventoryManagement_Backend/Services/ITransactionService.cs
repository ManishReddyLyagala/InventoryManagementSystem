using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagement_Backend.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllAsync();
        Task<TransactionDto?> GetByIdAsync(int id);
        Task<TransactionCreateDto> CreateAsync(TransactionCreateDto transaction);
        Task<TransactionDto?> UpdateAsync(int id, TransactionUpdateDto transaction);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TransactionDto>> FilterAsync(
            string? type,
            DateTime? date,
            TransactionStatus? status,
            int? productId,
            int? supplierId,
            int? userId);
        Task<IEnumerable<TransactionDto>> GetByUserIdAsync(int userId);
    }
}
