using InventoryManagement_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement_Backend.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Category { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public int Quantity { get; set; } = 0;
        [MaxLength(255)]
        public string? ImageUrl { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Price { get; set; }

        // FK
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        // Navigation
        public ICollection<PurchaseSalesOrders> PurchaseSalesOrders { get; set; } = new List<PurchaseSalesOrders>();
    }

}
