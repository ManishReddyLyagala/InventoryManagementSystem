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

        public async Task<IEnumerable<TransactionCreateDto>> GetAllAsync()
        {
            return await _context.Transactions
               .Select(t => new TransactionCreateDto
               {
                   Type = t.Type,
                   DateTime = t.DateTime,
                   SupplierId = t.SupplierId,
                   CustomerId = t.CustomerId
               })
               .ToListAsync();


        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions

                .FirstOrDefaultAsync(t => t.TransactionId == id);
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> UpdateAsync(int id, Transaction transaction)
        {
            var existing = await _context.Transactions.FindAsync(id);
            if (existing == null) return null;

            existing.Type = transaction.Type;
            existing.SupplierId = transaction.SupplierId;
            existing.CustomerId = transaction.CustomerId;
            existing.DateTime = transaction.DateTime;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Transaction>> FilterAsync(char? type, DateTime? date, int? customerId, int? supplierId)
        {
            var query = _context.Transactions
                .Include(t => t.PurchaseSalesOrders)
                .AsQueryable();

            if (type.HasValue)
                query = query.Where(t => t.Type == type);
            if (date.HasValue)
                query = query.Where(t => t.DateTime == date);

            if (customerId.HasValue)
                query = query.Where(t => t.CustomerId == customerId.Value);

            if (supplierId.HasValue)
                query = query.Where(t => t.SupplierId == supplierId.Value);

            return await query.ToListAsync();
            
        }

       




    }
}
