using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockAlertController : ControllerBase
    {
        private readonly IStockAlertService _stockAlertService;

        public StockAlertController(IStockAlertService stockAlertService)
        {
           _stockAlertService = stockAlertService;
        }
        [Authorize]
        [HttpGet("lowstock")]
        public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 5)
        {
            var products = await _stockAlertService.GetLowStockProductsAsync(threshold);
            return Ok(products);
        }

        [Authorize]
        [HttpPost("Send")]
        public async Task<IActionResult> SendLowStockEmail([FromQuery] int threshold = 5)
        {
            var products = await _stockAlertService.GetLowStockProductsAsync(threshold);
            await _stockAlertService.SendLowStockEmailAsync(products);
            return Ok(new { Message = "Low stock email sent successfully", Products = products });
        }



    }
}
