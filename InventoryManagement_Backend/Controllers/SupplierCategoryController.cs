using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryManagement.Dtos;
using InventoryManagement.Services;
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

        [HttpGet]
        public async Task<ActionResult<List<SupplierCategoryDto>>> GetAll()
        {
            return await _service.GetAllCategoriesAsync();
        }

        [HttpGet("{supplierId}")]
        public async Task<ActionResult<SupplierCategoryDto>> GetBySupplierId(int supplierId)
        {
            var result = await _service.GetBySupplierIdAsync(supplierId);
            if (result == null) return NotFound();
            return result;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateSupplierCategoryDto dto)
        {
            var updated = await _service.UpdateCategoryAsync(dto);
            if (!updated) return BadRequest();
            return Ok("Category updated successfully.");
        }
    }
}
