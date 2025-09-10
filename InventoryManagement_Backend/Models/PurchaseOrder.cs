using InventoryManagement_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement_Backend.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int SalesId { get; set; } // detail id (line id)

        [Required]
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
    }
}
