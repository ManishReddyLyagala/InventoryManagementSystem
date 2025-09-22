using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserByIDReadDto>> GetCustomer(int id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        //// GET: api/customers/{id}/transactions
        //[HttpGet("{id}/transactions")]
        //public async Task<ActionResult<IEnumerable<TransactionDto>>> GetCustomerTransactions(int id)
        //{
        //    var customer = await _service.GetCustomerByIdAsync(id);
        //    if (customer == null) return NotFound();
        //    return Ok(customer.Transactions);
        //}

        //// POST: api/customers
        //[HttpPost]
        //public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
        //{
        //    var customer = await _service.CreateCustomerAsync(dto);
        //    return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        //}

        //// PUT: api/customers/{id}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto dto)
        //{
        //    var success = await _service.UpdateCustomerAsync(id, dto);
        //    if (!success) return NotFound();
        //    return NoContent();
        //}

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _service.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
