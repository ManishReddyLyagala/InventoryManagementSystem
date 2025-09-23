using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagement.Dtos;
using InventoryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierCategoryController : ControllerBase
    {
        private readonly SupplierCategoryService _service;

        public SupplierCategoryController(SupplierCategoryService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("recalculate")]
        public async Task<IActionResult> RecalculateCategories()
        {
            await _service.UpdateSupplierCategoriesAsync();
            return Ok("Supplier categories recalculated successfully.");
        }
    }
    }
