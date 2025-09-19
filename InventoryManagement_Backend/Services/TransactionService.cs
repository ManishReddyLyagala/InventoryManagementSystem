using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using InventoryManagement_Backend.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement_Backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly InventoryDbContext _context;

        public TransactionService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            return await _context.Transactions
                .Include(t => t.PurchaseSalesOrders)
                .SelectMany(t => t.PurchaseSalesOrders, (t, o) => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    TransactionType = t.TransactionType,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status,
                    OrderId = o.OrderId,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    TotalAmount = o.TotalAmount,
                    SupplierId = o.SupplierId,
                    UserId = o.UserId
                })
                .ToListAsync();
        }

        public async Task<TransactionDto?> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid transaction ID.");

            return await _context.Transactions
                .Include(t => t.PurchaseSalesOrders)
                .Where(t => t.TransactionId == id)
                .SelectMany(t => t.PurchaseSalesOrders, (t, o) => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    TransactionType = t.TransactionType,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status,
                    OrderId = o.OrderId,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    TotalAmount = o.TotalAmount,
                    SupplierId = o.SupplierId,
                    UserId = o.UserId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TransactionDto>> GetByUserIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID.");

            return await _context.Transactions
                .Include(t => t.PurchaseSalesOrders)
                .Where(t => t.PurchaseSalesOrders.Any(po => po.UserId == userId))
                .SelectMany(t => t.PurchaseSalesOrders, (t, o) => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    TransactionType = t.TransactionType,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status,
                    OrderId = o.OrderId,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    TotalAmount = o.TotalAmount,
                    SupplierId = o.SupplierId,
                    UserId = o.UserId
                })
                .ToListAsync();
        }

        public async Task<TransactionCreateDto> CreateAsync(TransactionCreateDto transactionDto)
        {
            if (transactionDto == null) throw new ArgumentNullException(nameof(transactionDto));
            if (string.IsNullOrEmpty(transactionDto.TransactionType))
                throw new ArgumentException("TransactionType is required.");

            var transaction = new Transaction
            {
                TransactionType = transactionDto.TransactionType,
                TransactionDate = transactionDto.TransactionDate,
                Status = transactionDto.Status ?? TransactionStatus.Pending
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return new TransactionCreateDto
            {
                TransactionType = transaction.TransactionType,
                TransactionDate = transaction.TransactionDate,
                Status = transaction.Status
            };
        }

        public async Task<TransactionDto?> UpdateAsync(int id, TransactionUpdateDto transactionDto)
        {
            if (id <= 0) throw new ArgumentException("Invalid transaction ID.");
            if (transactionDto == null) throw new ArgumentNullException(nameof(transactionDto));

            //var existing = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == id);
            var existing = await _context.Transactions
                .Include(t => t.PurchaseSalesOrders)
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (existing == null) return null;

            if (!string.IsNullOrEmpty(transactionDto.TransactionType))
                existing.TransactionType = transactionDto.TransactionType;

            if (transactionDto.TransactionDate.HasValue)
                existing.TransactionDate = transactionDto.TransactionDate.Value;

            if (transactionDto.Status.HasValue)
                existing.Status = transactionDto.Status.Value;

            await _context.SaveChangesAsync();

            return new TransactionDto
            {
                TransactionId = existing.TransactionId,
                    TransactionType = existing.TransactionType,
                    TransactionDate = existing.TransactionDate,
                    Status = existing.Status,
                    //OrderId = existing.OrderId,
                    //ProductId = existing.ProductId,
                    //Quantity = existing.Quantity,
                    //TotalAmount = existing.TotalAmount,
                    //SupplierId = existing.SupplierId,
                    //UserId = existing.UserId,

            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid transaction ID.");

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TransactionDto>> FilterAsync(
            string? type,
            DateTime? date,
            TransactionStatus? status,
            int? productId,
            int? supplierId,
            int? userId)
        {
            var query = _context.Transactions
                .Include(t => t.PurchaseSalesOrders)
                .AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(t => t.TransactionType == type);

            if (date.HasValue)
                query = query.Where(t => t.TransactionDate.Date == date.Value.Date);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (productId.HasValue)
                query = query.Where(t => t.PurchaseSalesOrders.Any(po => po.ProductId == productId));

            if (userId.HasValue)
                query = query.Where(t => t.PurchaseSalesOrders.Any(po => po.UserId == userId));

            if (supplierId.HasValue)
                query = query.Where(t => t.PurchaseSalesOrders.Any(po => po.SupplierId == supplierId));

            return await query
                .SelectMany(t => t.PurchaseSalesOrders, (t, o) => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    TransactionType = t.TransactionType,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status,
                    OrderId = o.OrderId,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    TotalAmount = o.TotalAmount,
                    SupplierId = o.SupplierId,
                    UserId = o.UserId
                })
                .ToListAsync();
        }
    }
}
