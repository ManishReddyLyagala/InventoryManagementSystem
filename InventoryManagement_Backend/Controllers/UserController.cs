using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        // GET: api/user
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/user/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserByIDReadDto>> GetUser(int id)
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
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserByIDReadDto>> UpdateUserDetails(int id, UpdateCustomerDto dto)
        {
            var updatedUser = await _service.UpdateUserAsync(id, dto);
            if (updatedUser==null) return NotFound();
            return Ok(updatedUser);
        }

        // DELETE: api/user/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _service.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
