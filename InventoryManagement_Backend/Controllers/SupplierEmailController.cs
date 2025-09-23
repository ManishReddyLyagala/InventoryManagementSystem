using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierEmailController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly IEmailSender _emailSender;

        public SupplierEmailController(InventoryDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [Authorize]
        [HttpPost("send/{supplierId}")]
        public async Task<IActionResult> SendEmailToSupplier(int supplierId, [FromBody] SupplierEmailRequest request)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.SupplierId == supplierId);

            if (supplier == null)
                return NotFound("Supplier not found");

            if (string.IsNullOrEmpty(supplier.EmailID))
                return BadRequest("Supplier does not have an email address");

            try
            {
                var recipients = new List<string> { "charishmapaluri3904@gmail.com" };

                await _emailSender.SendEmailAsync(request.Subject, request.Body, recipients);

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }

        // DTO
        public class SupplierEmailRequest
        {
            public string Subject { get; set; } = string.Empty;
            public string Body { get; set; } = string.Empty;
        }
    }
}
