using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


//using InventoryManagement_Backend.Services;
using Microsoft.EntityFrameworkCore;
namespace InventoryManagement_Backend.Services
{
    public class PurchaseSalesOrdersServices : IPurchaseSalesOrdersService
    {
        private readonly InventoryDbContext _context;

        public PurchaseSalesOrdersServices(InventoryDbContext context)
        {
            _context = context;
        }

        // ========== CRUD ==========
        public async Task<IEnumerable<PurchaseSalesOrders>> GetAllAsync() =>
            await _context.PurchaseOrders
                .Include(p => p.Product)
                .Include(p => p.Supplier)
                .Include(p => p.Customer)
                .ToListAsync();

        public async Task<PurchaseSalesOrders?> GetByIdAsync(int id) =>
            await _context.PurchaseOrders
                .Include(p => p.Product)
                .Include(p => p.Supplier)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(o => o.OrderId == id);

        public async Task<IEnumerable<PurchaseSalesOrders>> GetByTypeAsync(string type) =>
            await _context.PurchaseOrders
                .Where(o => o.OrderType == type)
                .Include(p => p.Product)
                .Include(p => p.Supplier)
                .Include(p => p.Customer)
                .ToListAsync();

        public async Task<List<PurchaseSalesOrders>> CreateOrderAsync(OrderCreateRequest orderRequest)
        {
            var CreatedOrders = new List<PurchaseSalesOrders>();

            foreach (var item in orderRequest.Items) {
                
                var order = new PurchaseSalesOrders
                {
                    TransactionId = orderRequest.TransactionId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalAmount = item.TotalAmount,
                    OrderType = orderRequest.OrderType,
                    SupplierId =   orderRequest.SupplierId,
                    CustomerId =   orderRequest.CustomerId,
                    OrderDate = DateTime.UtcNow
                };
                _context.PurchaseOrders.Add(order);
                CreatedOrders.Add(order);
            }
            await _context.SaveChangesAsync();
            return CreatedOrders;
            
        }

        public async Task<PurchaseSalesOrders?> UpdateAsync(int id, PurchaseSalesOrderDto order)
        {
            var existing = await _context.PurchaseOrders.FindAsync(id);
            if (existing == null) return null;

            existing.TransactionId = order.TransactionId;
            existing.ProductId = order.ProductId;
            existing.Quantity = order.Quantity;
            existing.TotalAmount = order.TotalAmount;
            existing.OrderType = order.OrderType;
            existing.SupplierId = order.SupplierId;
            existing.CustomerId = order.CustomerId;
            existing.OrderDate = order.OrderDate;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.PurchaseOrders.FindAsync(id);
            if (order == null) return false;

            _context.PurchaseOrders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        // ========== Analytics ==========
        public async Task<decimal> GetTotalSalesAsync() =>
            await _context.PurchaseOrders
                .Where(o => o.OrderType == "S")
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

        public async Task<decimal> GetTotalPurchasesAsync() =>
            await _context.PurchaseOrders
                .Where(o => o.OrderType == "P")
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

        public async Task<decimal> GetProfitAsync()
        {
            var sales = await GetTotalSalesAsync();
            var purchases = await GetTotalPurchasesAsync();
            return sales - purchases;
        }

        public async Task<IEnumerable<object>> GetMonthlyTrendsAsync(int year)
        {
            return await _context.PurchaseOrders
                .Where(o => o.OrderDate.Year == year)
                .GroupBy(o => new { o.OrderDate.Month, o.OrderType })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    OrderType = g.Key.OrderType,
                    TotalOrders = g.Count(),
                    TotalAmount = (decimal?)g.Sum(o => o.TotalAmount) ?? 0
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetYearlyTrendsAsync()
        {
            return await _context.PurchaseOrders
                .GroupBy(o => new { o.OrderDate.Year, o.OrderType })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    OrderType = g.Key.OrderType,
                    TotalOrders = g.Count(),
                    TotalAmount = (decimal?)g.Sum(o => o.TotalAmount) ?? 0
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetWeeklyTrendsAsync()
        {
            var weekStartDate = DateTime.UtcNow.Date.AddDays(-7);
            return await _context.PurchaseOrders.Where(o => o.OrderDate >= weekStartDate)
                .GroupBy(o => new { o.OrderType })
                .Select(g => new
                {
                    OrderType = g.Key.OrderType,
                    TotalOrders = g.Count(),
                    TotalAmount = (decimal?)g.Sum(o => o.TotalAmount) ?? 0
                }).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetTodayTrendsAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.PurchaseOrders.Where(o => o.OrderDate == today)
                .GroupBy(o => o.OrderType )
                .Select(g => new
                {
                    Today = today,
                    OrderType = g.Key,
                    TotalOrders = g.Count(),
                    TotalAmount = (decimal?)g.Sum(o => o.TotalAmount)?? 0
                }).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetCustomTrendsAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.PurchaseOrders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .GroupBy(o => o.OrderType)
                .Select(g => new
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    OrderType = g.Key,
                    TotalOrders = g.Count(),
                    TotalAmount = (decimal?)g.Sum(o=>o.TotalAmount) ?? 0
                }).ToListAsync();
        }
        
    }
}
