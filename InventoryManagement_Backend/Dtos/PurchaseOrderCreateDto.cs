using InventoryManagement_Backend.Models;

namespace InventoryManagement_Backend.Dtos
{
    public class PurchaseOrderCreate
    {
        public int SalesId { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        //public string OrderType { get; set; } 
        //public int? SupplierId { get; set; }
        //public int? CustomerId { get; set; }
        //public DateTime OrderDate { get; set; }

        // For readability in response
        public string? ProductName { get; set; }
        public string? SupplierName { get; set; }
        public string? CustomerName { get; set; }
        //public ProductCreateDto product { get; set; } = new();
    }
}
