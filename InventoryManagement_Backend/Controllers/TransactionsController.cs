using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var transactions = await _transactionService.GetAllAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetByIdAsync(id);
                if (transaction is null) return NotFound();
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var transactions = await _transactionService.GetByUserIdAsync(userId);
                if (!transactions.Any()) return NotFound("No transactions found for this user.");
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionCreateDto transaction)
        {
            try
            {
                var created = await _transactionService.CreateAsync(transaction);
                return CreatedAtAction(nameof(GetById), new { id = created.TransactionType }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TransactionCreateDto transaction)
        {
            try
            {
                var updated = await _transactionService.UpdateAsync(id, transaction);
                if (updated is null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _transactionService.DeleteAsync(id);
                if (!success) return NotFound();
                return Ok("Transaction deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> FilterTransactions(
            string? type,
            DateTime? date,
            TransactionStatus? status,
            int? productId,
            int? supplierId,
            int? userId)
        {
            try
            {
                var transactions = await _transactionService.FilterAsync(type, date, status, productId, supplierId, userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
