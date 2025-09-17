using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement_Backend.Services.Interfaces;
using InventoryManagement_Backend.Models;
namespace InventoryManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionService.GetAllAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _transactionService.GetByIdAsync(id);
            if (transaction is null) return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transaction transaction)
        {
            var created = await _transactionService.CreateAsync(transaction);
            return CreatedAtAction(nameof(GetById), new { id = created.TransactionId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Transaction transaction)
        {
            var updated = await _transactionService.UpdateAsync(id, transaction);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _transactionService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterTransactions(
            string? type,
            DateTime? date,
            int? customerId,
            int? supplierId)
        {
            char? typeChar = null;

            if (!string.IsNullOrEmpty(type) && type.Length == 1)
                typeChar = type[0];

            var transactions = await _transactionService.FilterAsync(typeChar, date, customerId, supplierId);
            return Ok(transactions);
        }
        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyReport()
        {
            var today = DateTime.UtcNow.Date;

            var (Purchases,  Sales,  noOfpurchases,  noOfsales) = await _transactionService.GetDailyReportAsync(today);

            var report = new
            {
                Date = today.ToString("yyyy-MM-dd"),
                TotalPurchases = Purchases,
                TotalSales = Sales,
                noOfpurchases = noOfpurchases,
                noOfsales = noOfsales
            };

            return Ok(report);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int? year, [FromQuery] int? month)
        {
            var y = year ?? DateTime.UtcNow.Year;
            var m = month ?? DateTime.UtcNow.Month;

            var report = await _transactionService.GetMonthlyReportAsync(y, m);

            return Ok(new
            {
                Year = y,
                Month = m,
                Breakdown = report
            });
        }

        [HttpGet("reports/yearly/{year}")]
        public async Task<IActionResult> GetYearlyReport(int year)
        {
            try
            {
                var report = await _transactionService.GetYearlyReportAsync(year);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
