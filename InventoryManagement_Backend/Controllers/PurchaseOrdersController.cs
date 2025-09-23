using Hangfire.Server;
using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement_Backend.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {


        private readonly IPurchaseSalesOrdersService _service;
        private readonly InventoryDbContext _dbContext;
        private readonly IInvoiceEmailService _invoiceEmailService;

        public PurchaseOrdersController(IPurchaseSalesOrdersService service, InventoryDbContext dbContext, IInvoiceEmailService invoiceEmailService)
        {
            _service = service;
            _dbContext = dbContext;
            _invoiceEmailService = invoiceEmailService;
        }

        private PurchaseSalesOrderDto MapToDto(PurchaseSalesOrders order)
        {
            return new PurchaseSalesOrderDto
            {
                OrderId = order.OrderId,
                TransactionId = order.TransactionId,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                TotalAmount = order.TotalAmount,
                OrderType = order.OrderType,
                SupplierId = order.SupplierId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ProductName = order.Product?.Name,
                SupplierName = order.Supplier?.Name,
                UserName = order.User?.Name
            };
        }

        private PurchaseSalesOrders MapFromDto(PurchaseSalesOrderDto orderDto)
        {
            return new PurchaseSalesOrders
            {
                OrderId = orderDto.OrderId,
                TransactionId = orderDto.TransactionId,
                ProductId = orderDto.ProductId,
                Quantity = orderDto.Quantity,
                TotalAmount = orderDto.TotalAmount,
                OrderType = orderDto.OrderType,
                SupplierId = orderDto.SupplierId,
                UserId = orderDto.UserId,
                OrderDate = orderDto.OrderDate
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet] // GET: api/PurchaseSalesOrders
        public async Task<ActionResult<IEnumerable<PurchaseSalesOrderDto>>> GetAllOrders()
        {
            try
            {
                var orders = await _service.GetAllAsync();
                if (orders == null)
                {
                    return NotFound("No Orders Found");
                }
                return Ok(orders.Select(MapToDto));
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error fetching orders: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("id")] // GET: api/PurchaseSalesOrders/{id}
        public async Task<ActionResult<PurchaseSalesOrderDto>> GetOrderById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid order id");
                }
                var order = await _service.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound("No Order Found");
                }
                return Ok(MapToDto(order));
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error fetching orders: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("myOrders/{userId}")]
        public async Task<ActionResult<IEnumerable<PurchaseSalesOrderDto>>> GetUserOrdersById(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid User id");
            }
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("No User Found");
            }
            var myOrders = await _service.GetUserOrders(userId);
            if (myOrders == null || !myOrders.Any()) // Added a check for an empty list
            {
                return NotFound("No Order Found");
            }

            return Ok(myOrders.Select(MapToDto));
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<IEnumerable<PurchaseSalesOrderDto>>> CreateOrder(OrderCreateRequest orderRequest)
        {
            try { 
            if (orderRequest == null)
                return BadRequest("Request body cannot be null");

            if (!((orderRequest.OrderType == "P" && orderRequest.SupplierId > 0 && (orderRequest.UserId == null || orderRequest.UserId <= 0)) ||
       (orderRequest.OrderType == "S" && orderRequest.UserId > 0 && (orderRequest.SupplierId == null || orderRequest.SupplierId <= 0))))
            {
                return BadRequest("OrderType and Supplier/Customer Id missmatch. recheck");
            }
            if (orderRequest.Items == null || !orderRequest.Items.Any())
                return BadRequest("At least one product item must be included in the order.");

            // transaction should be callled
                var transaction = await _dbContext.Transactions.FindAsync(orderRequest.TransactionId);
                var customer = orderRequest.UserId.HasValue
                                ? await _dbContext.User.FindAsync(orderRequest.UserId.Value)
                                : null;
                var supplier = orderRequest.SupplierId.HasValue
                                ? await _dbContext.Suppliers.FindAsync(orderRequest.SupplierId.Value)
                                : null;
                if (!(transaction!=null && (customer!=null || supplier!=null)))
                {
                    return BadRequest("Invalid Id's detals missmatch.");
                }

                var createdOrders = await _service.CreateOrderAsync(orderRequest);

                if (createdOrders == null || !createdOrders.Any())
                    return BadRequest("Failed to create order. Please check product details.");
                // should dec quantity

                var result = createdOrders.Select(o => MapToDto(o));
                

                var productIds = createdOrders.Select(o => o.ProductId).ToList();
                var products = await _dbContext.Products
                                    .Where(p => productIds.Contains(p.ProductId))
                                    .ToListAsync();

                // Send Invoice Email (one email with multiple products)
                await _invoiceEmailService.SendOrderInvoiceEmailAsync(customer, supplier, createdOrders, transaction, products);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating order: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<PurchaseSalesOrderDto>>> GetOrderByType(string type)
        {
            try
            {
                Console.WriteLine("################################################ " + type);
                if (!type.Equals("P") && !type.Equals("S"))
                {
                    return BadRequest("Invalid type. order type can be 'P' i.e Purchase / 'S' i.e Sales");
                }

                var orders = await _service.GetByTypeAsync(type);
                if (orders == null)
                {
                    return NotFound("No Orders Found");
                }
                return Ok(orders.Select(MapToDto));
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error fetching orders by type: {ex.Message}");
            }
           
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<PurchaseSalesOrderDto>> UpdateOrders(int id, PurchaseSalesOrderDto orderDto)
        {
            try
            {
                if (id != orderDto.OrderId)
                {
                    return BadRequest("order id in url and body are missmatching.");
                }
                if (orderDto.ProductId <= 0)
                    return BadRequest("Invalid ProductId");

                if (!orderDto.OrderType.Equals("P") && !orderDto.OrderType.Equals("S"))
                {
                    return BadRequest("Invalid type. order type can be 'P' i.e Purchase / 'S' i.e Sales");
                }

                if (orderDto.Quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than zero.");
                }
                if (orderDto.TotalAmount <= 0)
                {
                    return BadRequest("Total Amount must be greater than zero.");
                }
                if (DateTime.Now.Date != orderDto.OrderDate.Date)
                {
                    return BadRequest("Date should be the current date.");
                }
                if (!((orderDto.OrderType == "P" && orderDto.SupplierId > 0 && (orderDto.UserId == null || orderDto.UserId <= 0)) ||
                        (orderDto.OrderType == "S" && orderDto.UserId > 0 && (orderDto.SupplierId == null || orderDto.SupplierId <= 0))))
                {
                    return BadRequest("OrderType and Supplier/Customer Id missmatch. recheck");
                }
                if (orderDto.OrderType == "P" && orderDto.SupplierId > 0)
                {
                    var supplierDetails = await _dbContext.Suppliers.FindAsync(orderDto.SupplierId);
                    if (supplierDetails == null)
                    {
                        return NotFound("Supplier with this id not found");
                    }
                }
                else
                {
                    var customerDetails = await _dbContext.User.FindAsync(orderDto.UserId);
                    if (customerDetails == null)
                    {
                        return NotFound("Customer with this id not found");
                    }
                }

                var updatedOrder = await _service.UpdateAsync(id, orderDto);
                if (updatedOrder == null)
                {
                    return NotFound($"No order found for this OrderId : {id}");
                }
                return Ok(updatedOrder);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error updating order: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteOrder(int id)
        {
            try
            {

                if (id <= 0) return BadRequest("Invalid Order ID");
                var deleteStatus = await _service.DeleteAsync(id);
                if (!deleteStatus)
                {
                    return BadRequest("Can't delete because of wrong orderid.");
                }
                return Ok("Successfully Order deleted");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error deleting order: {ex.Message}");
            }
        }

        // ANALYTICS
        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/totalsales")]
        public async Task<ActionResult<decimal>> GetTotalSales()
        {
            try
            {
                return Ok(await _service.GetTotalSalesAsync());
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error fetching total sales: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/totalpurchases")]
        public async Task<ActionResult<decimal>> GetTotalPurchases()
        {
            try
            {
                return await _service.GetTotalPurchasesAsync();
            }
            catch (Exception ex) {

                return StatusCode(500, $"Error fetching total purchase: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/profits")]
        public async Task<ActionResult<decimal>> GetProfits()
        {
            try
            {
                return Ok(await _service.GetProfitAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching profits: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/monthly/{year}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMothlyReport([Range(2000, 3000)] int year)
        {
            try
            {
                var report = await _service.GetMonthlyTrendsAsync(year);
                return Ok(report ?? new List<object>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching monthly report: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/yearly")]
        public async Task<ActionResult<IEnumerable<object>>> GetYearlyReport()
        {
            try
            {
                return Ok(await _service.GetYearlyTrendsAsync() ?? new List<object>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching yearly report: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/LastSevenDays")]
        public async Task<ActionResult<IEnumerable<object>>> GetWeeklyReport()
        {
            try
            {
                return Ok(await _service.GetWeeklyTrendsAsync() ?? new List<object>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching weekly report: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/today")]
        public async Task<ActionResult<IEnumerable<object>>> GetTodayReport()
        {
            try
            {
                return Ok(await _service.GetTodayTrendsAsync() ?? new List<object>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching today report: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/custom")]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomDatesReport(DateTime startDate, DateTime endDate)
        {
            try {
                if (!(DateTime.TryParse(startDate.ToString(), out startDate) && DateTime.TryParse(endDate.ToString(), out endDate))) {
                    return BadRequest("Invalid DateTime Format.");
                }
                if (endDate < startDate)
                {
                    return BadRequest("End date must be after start date");
                }
                return Ok(await _service.GetCustomTrendsAsync(startDate, endDate));
            }catch(Exception ex)
            {
                return StatusCode(500, $"Error fetching custom report: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("analytics/ProductsSold")]
        public async Task<ActionResult<int>> GetTotalProductsSold()
        {
            try
            {
                return Ok(await _service.GetTotalProductsSold());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching custom report: {ex.Message}");
            }
        }
    }
}
