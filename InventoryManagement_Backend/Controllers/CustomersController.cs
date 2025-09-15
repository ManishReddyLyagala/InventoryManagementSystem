using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _service.GetAllCustomersAsync();
            return Ok(customers);
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDetailDto>> GetCustomer(int id)
        {
            var customer = await _service.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // GET: api/customers/{id}/transactions
        [HttpGet("{id}/transactions")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetCustomerTransactions(int id)
        {
            var customer = await _service.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer.Orders);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
        {
            var customer = await _service.CreateCustomerAsync(dto);
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto dto)
        {
            var success = await _service.UpdateCustomerAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var success = await _service.DeleteCustomerAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
