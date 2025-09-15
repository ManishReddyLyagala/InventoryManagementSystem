using InventoryManagement_Backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement_Backend.Services;


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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierReadDto>>> GetSuppliers()
        {
            return Ok(await _supplierService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierReadDto>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierReadDto>> PostSupplier(SupplierCreateDto dto)
        {
            var created = await _supplierService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetSupplier), new { id = created.SupplierId }, created);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PutSupplier(int id, SupplierUpdateDto dto)
        {
            var updated = await _supplierService.PatchAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var deleted = await _supplierService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
