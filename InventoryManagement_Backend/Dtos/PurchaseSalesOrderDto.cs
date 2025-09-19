using InventoryManagement_Backend.Models;

namespace InventoryManagement_Backend.Dtos
{
    public class OrderCreateRequest
    {
        public int TransactionId { get; set; }
        public string OrderType { get; set; } // "Sales" or "Purchase"
        public int? SupplierId { get; set; }
        public int? UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }


    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class PurchaseSalesOrderDto
    {
        public int OrderId { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderType { get; set; } // "Sales" or "Purchase"
        public int? SupplierId { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; }

        // For readability in response
        public string? ProductName { get; set; }
        public string? SupplierName { get; set; }
        public string? CustomerName { get; set; }
        public ProductCreateDto Product { get;  set; }
        

    }
}