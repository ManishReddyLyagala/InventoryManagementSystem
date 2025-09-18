using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using InventoryManagement_Backend.Dtos;

namespace InventoryManagement_Backend.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly InventoryDbContext _context;

        public TransactionService(InventoryDbContext context)
        {
            _context = context;
        }

        // Get all transactions with orders
        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            try
            {
                return await _context.Transactions
                    .Include(t => t.PurchaseSalesOrders)
                    .SelectMany(t => t.PurchaseSalesOrders, (t, o) => new TransactionDto
                    {
                        TransactionId = t.TransactionId,
                        TransactionType = t.TransactionType,
                        TransactionDate = t.TransactionDate,
                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Quantity = o.Quantity,
                        TotalAmount = o.TotalAmount,
                        SupplierId = o.SupplierId,
                        CustomerId = o.CustomerId
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log ex here
                throw new ApplicationException("Error fetching transactions.", ex);
            }
        }

        // Get a transaction by id
        public async Task<TransactionDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid transaction ID.");

            try
            {
                return await _context.Transactions
                    .Include(t => t.PurchaseSalesOrders)
                    .Where(t => t.TransactionId == id)
                    .SelectMany(t => t.PurchaseSalesOrders, (t, o) => new TransactionDto
                    {
                        TransactionId = t.TransactionId,
                        TransactionType = t.TransactionType,
                        TransactionDate = t.TransactionDate,
                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Quantity = o.Quantity,
                        TotalAmount = o.TotalAmount,
                        SupplierId = o.SupplierId,
                        CustomerId = o.CustomerId
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching transaction with ID {id}.", ex);
            }
        }

        // Create a new transaction
        public async Task<TransactionDto> CreateAsync(TransactionCreateDto transactionDto)
        {
            if (transactionDto == null)
                throw new ArgumentNullException(nameof(transactionDto));

            if (string.IsNullOrEmpty(transactionDto.TransactionType))
                throw new ArgumentException("TransactionType is required.");

            try
            {
                var transaction = new Transaction
                {
                    TransactionType = transactionDto.TransactionType,
                    TransactionDate = transactionDto.TransactionDate
                };

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return new TransactionDto
                {
                    TransactionId = transaction.TransactionId,
                    TransactionType = transaction.TransactionType,
                    TransactionDate = transaction.TransactionDate
                };
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("Error saving transaction to database.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unexpected error creating transaction.", ex);
            }
        }

        // Update transaction
        public async Task<TransactionDto?> UpdateAsync(int id, TransactionCreateDto transactionDto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid transaction ID.");

            if (transactionDto == null)
                throw new ArgumentNullException(nameof(transactionDto));

            try
            {
                var existing = await _context.Transactions
                    .FirstOrDefaultAsync(t => t.TransactionId == id);

                if (existing == null)
                    return null;

                existing.TransactionType = transactionDto.TransactionType;
                existing.TransactionDate = transactionDto.TransactionDate;

                await _context.SaveChangesAsync();

                return new TransactionDto
                {
                    TransactionId = existing.TransactionId,
                    TransactionType = existing.TransactionType,
                    TransactionDate = existing.TransactionDate
                };
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException($"Error updating transaction with ID {id}.", ex);
            }
        }

        // Delete transaction
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid transaction ID.");

            try
            {
                var transaction = await _context.Transactions.FindAsync(id);
                if (transaction == null) return false;

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException($"Error deleting transaction with ID {id}.", ex);
            }
        }

        // Filter transactions
        public async Task<IEnumerable<TransactionDto>> FilterAsync(
            string? type,
            DateTime? date,
            int? productId,
            int? supplierId,
            int? customerId)
        {
            try
            {
                var query = _context.Transactions
                    .Include(t => t.PurchaseSalesOrders)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(type))
                    query = query.Where(t => t.TransactionType == type);
                if (date.HasValue)
                    query = query.Where(t => t.TransactionDate.Date == date.Value.Date);

                if (productId.HasValue)
                    query = query.Where(t => t.PurchaseSalesOrders.Any(po => po.ProductId == productId));
                if (customerId.HasValue)
                    query = query.Where(t => t.PurchaseSalesOrders.Any(po => po.CustomerId == customerId));
                if (supplierId.HasValue)
                    query = query.Where(t => t.PurchaseSalesOrders.Any(po => po.SupplierId == supplierId));

                return await query
                    .SelectMany(t => t.PurchaseSalesOrders, (t, o) => new TransactionDto
                    {
                        TransactionId = t.TransactionId,
                        TransactionType = t.TransactionType,
                        TransactionDate = t.TransactionDate,
                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Quantity = o.Quantity,
                        TotalAmount = o.TotalAmount,
                        SupplierId = o.SupplierId,
                        CustomerId = o.CustomerId
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error filtering transactions.", ex);
            }
        }
    }
}
