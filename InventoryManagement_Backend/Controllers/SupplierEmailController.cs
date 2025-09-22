using InventoryManagement_Backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace InventoryManagement_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierEmailController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public SupplierEmailController(InventoryDbContext context)
        {
            _context = context;
        }

        [HttpPost("send/{supplierId}")]
        public async Task<IActionResult> SendEmailToSupplier(int supplierId, [FromBody] SupplierEmailRequest request)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.SupplierId == supplierId);

            if (supplier == null)
                return NotFound("Supplier not found");

            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress("yourgmail@gmail.com"),
                    Subject = request.Subject,
                    Body = $"{request.Body}"
                };
                message.To.Add("charishmapaluri3904@gmail.com");

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("charishmapaluri@gmail.com", "gieaulrxbdrtdynx");
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }

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
