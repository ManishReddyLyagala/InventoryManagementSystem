using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace InventoryManagement_Backend.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string MobileNumber { get; set; }
        [Required]
        [EmailAddress]
        public string EmailID { get; set; }
        public string ProductCategory { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Transaction> PurchaseTransactions { get; set; } = new List<Transaction>();
    }
}
