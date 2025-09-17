using InventoryManagement_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement_Backend.Models
{
    public class PurchaseSalesOrders
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalAmount must be greater than 0.")]
        public decimal TotalAmount { get; set; }

        [Required]
        [RegularExpression("^(S|P)$", ErrorMessage = "OrderType must be either 'Sales' or 'Purchase'.")]
        public string OrderType { get; set; } // "S" or "P"

        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Product Product { get; set; }
        public Supplier Supplier { get; set; }
        public Customer Customer { get; set; }
        public Transaction Transaction { get; set; }
    }
}
