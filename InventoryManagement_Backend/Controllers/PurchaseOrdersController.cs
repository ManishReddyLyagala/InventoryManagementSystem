using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {


        private readonly IPurchaseSalesOrdersService _service;

        public PurchaseOrdersController(IPurchaseSalesOrdersService service)
        {
            _service = service;
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
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                ProductName = order.Product?.Name,
                SupplierName = order.Supplier?.Name,
                CustomerName = order.Customer?.Name
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
                CustomerId = orderDto.CustomerId,
                OrderDate = orderDto.OrderDate
            };
        }

        [HttpGet] // GET: api/PurchaseSalesOrders
        public async Task<ActionResult<IEnumerable<PurchaseSalesOrderDto>>> GetAllOrders()
        {
            var orders = await _service.GetAllAsync();
            if (orders == null)
            {
                return NotFound("No Orders Found");
            }
            return Ok(orders.Select(MapToDto));
        }

        [HttpGet("id")] // GET: api/PurchaseSalesOrders/{id}
        public async Task<ActionResult<PurchaseSalesOrderDto>> GetOrderById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if(order == null)
            {
                return NotFound("No Order Found");
            }
            return Ok(MapToDto(order));
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<IEnumerable<PurchaseSalesOrderDto>>> CreateOrder(OrderCreateRequest orderRequest)
        {
            if (!((orderRequest.OrderType == "P" && orderRequest.SupplierId > 0 && (orderRequest.CustomerId == null || orderRequest.CustomerId <= 0)) ||
       (orderRequest.OrderType == "S" && orderRequest.CustomerId > 0 && (orderRequest.SupplierId == null || orderRequest.SupplierId <= 0))))
            {
                return BadRequest("OrderType and Supplier/Customer Id missmatch. recheck");
            }
            var createdOrders = await _service.CreateOrderAsync(orderRequest);
            var result = createdOrders.Select(o => MapToDto(o));
           
            return Ok(result);
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<PurchaseSalesOrderDto>>> GetOrderByType(string type)
        {
            Console.WriteLine("################################################ "+ type);
            if(!type.Equals("P") && !type.Equals("S"))
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

        [HttpPut("{id}")]
        public async Task<ActionResult<PurchaseSalesOrderDto>> UpdateOrders(int id, PurchaseSalesOrderDto orderDto)
        {
            if (id != orderDto.OrderId)
            {
                return BadRequest("order id in url and body are missmatching.");
            }
            var updatedOrder = await _service.UpdateAsync(id, orderDto);
            if (updatedOrder == null)
            {
                return NotFound($"no order found for this OrderId : {id}");
            }
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteOrder(int id)
        {
            var deleteStatus = await _service.DeleteAsync(id);
            if (!deleteStatus)
            {
                return BadRequest("can't delete because of wrong orderid.");
            }
            return Ok("Successfully Order deleted");
        }

        // ANALYTICS

        [HttpGet("analytics/totalsales")]
        public async Task<ActionResult<decimal>> GetTotalSales()
        {
            return Ok(await _service.GetTotalSalesAsync());
        }

        [HttpGet("analytics/totalpurchases")]
        public async Task<ActionResult<decimal>> GetTotalPurchases()
        {
            return await _service.GetTotalPurchasesAsync();
        }

        [HttpGet("analytics/profits")]
        public async Task<ActionResult<decimal>> GetProfits()
        {
            return Ok(await _service.GetProfitAsync());
        }

        [HttpGet("analytics/monthly/{year}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMothlyReport(int year)
        {
            return Ok(await _service.GetMonthlyTrendsAsync(year));
        }

        [HttpGet("analytics/yearly")]
        public async Task<ActionResult<IEnumerable<object>>> GetYearlyReport()
        {
            return Ok(await _service.GetYearlyTrendsAsync());
        }

        [HttpGet("analytics/LastSevenDays")]
        public async Task<ActionResult<IEnumerable<object>>> GetWeeklyReport()
        {
            return Ok(await _service.GetWeeklyTrendsAsync());
        }

        [HttpGet("analytics/today")]
        public async Task<ActionResult<IEnumerable<object>>> GetTodayReport()
        {
            return Ok(await _service.GetTodayTrendsAsync());
        }

        [HttpGet("analytics/custom")]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomDatesReport( DateTime startDate, DateTime endDate)
        {

            if (endDate < startDate)
            {
                return BadRequest("End date must be after start date");
            }
            return Ok(await _service.GetCustomTrendsAsync(startDate, endDate));
        }
    }
}
