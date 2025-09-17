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

        public async Task<(decimal Purchases, decimal Sales, decimal NoOfPurchases, decimal NoOfSales)> GetDailyReportAsync(DateTime date)
        {
            var purchases = await _context.Transactions
                .Where(t => t.Type == 'P' && t.DateTime.Date == date.Date)
                .SelectMany(t => t.PurchaseSalesOrders)
                .SumAsync(po => (decimal?)po.TotalAmount ?? 0);

            var sales = await _context.Transactions
                .Where(t => t.Type == 'S' && t.DateTime.Date == date.Date)
                .SelectMany(t => t.PurchaseSalesOrders)
                .SumAsync(so => (decimal?)so.TotalAmount ?? 0);

            var noOfpurchases = await _context.Transactions
                .CountAsync(t => t.Type == 'P' && t.DateTime.Date == date.Date);

            var noOfsales = await _context.Transactions
                .CountAsync(t => t.Type == 'S' && t.DateTime.Date == date.Date);

            return (purchases, sales, noOfpurchases, noOfsales);
        }

        public async Task<IEnumerable<object>> GetMonthlyReportAsync(int year, int month)
        {
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1);

                var report = await _context.Transactions
                    .Where(t => t.DateTime >= startDate && t.DateTime <= endDate)
                    .GroupBy(t => t.DateTime.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Purchases = g.Where(t => t.Type == 'P')
                                     .SelectMany(t => t.PurchaseSalesOrders)
                                     .Sum(po => (decimal?)po.TotalAmount ?? 0),

                        Sales = g.Where(t => t.Type == 'S')
                                 .SelectMany(t => t.PurchaseSalesOrders) 
                                 .Sum(so => (decimal?)so.TotalAmount ?? 0),

                        NoOfPurchases = g.Count(t => t.Type == 'P'),
                        NoOfSales = g.Count(t => t.Type == 'S')
                    })
                    .OrderBy(r => r.Date)
                    .ToListAsync();

                return report;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while generating monthly report: {ex.Message}");
                return Enumerable.Empty<object>();
            }

        }

        public async Task<IEnumerable<object>> GetYearlyReportAsync(int year)
        {
            var report = await _context.Transactions
                .Where(t => t.DateTime.Year == year)
                .GroupBy(t => new { t.DateTime.Month, t.Type })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Type = g.Key.Type,
                    TotalAmount = g.Sum(t =>
                        t.Type == 'P'
                            ? t.PurchaseSalesOrders.Sum(po => po.TotalAmount)
                            : t.PurchaseSalesOrders.Sum(so => so.TotalAmount)),
                    Count = g.Count()
                })
                .OrderBy(r => r.Month)
                .ToListAsync();

            return report;
        }




    }
}
