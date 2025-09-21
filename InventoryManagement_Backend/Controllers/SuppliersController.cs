using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InventoryManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        //[Authorize (Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierReadDto>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllAsync();
            if (suppliers == null || !suppliers.Any())
                return NotFound(new { message = "No suppliers found. eiruyiuewro" });
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierReadDto>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null) return NotFound(new { message = $"Supplier with ID {id} not found." });
            return Ok(supplier);
        }

        [HttpPost("add_supplier")]
        public async Task<ActionResult<SupplierReadDto>> PostSupplier(SupplierCreateDto dto)
        {
            var created = await _supplierService.CreateAsync(dto);
            if (created == null)
                return BadRequest(new { message = "Failed to add supplier." });
            return CreatedAtAction(nameof(GetSupplier), new { id = created.SupplierId }, created);
        }

        [HttpPatch("update_supplier/{id}")]
        public async Task<IActionResult> PutSupplier(int id, SupplierUpdateDto dto)
        {
            var updated = await _supplierService.PatchAsync(id, dto);
            if (!updated)
                return NotFound(new { message = $"Failed to update supplier with ID {id}." });
            return Ok(new { message = $"Supplier with ID {id} updated successfully." });
        }

        [HttpDelete("remove_supplier/{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var deleted = await _supplierService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Failed to delete supplier with ID {id}." });
            return Ok(new { message = $"Supplier with ID {id} deleted successfully." });
        }

    }
}
