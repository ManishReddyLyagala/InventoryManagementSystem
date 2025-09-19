using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace InventoryManagement_Backend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? MobileNumber { get; set; }
        [Required]
        [EmailAddress]
        public string EmailID { get; set; }
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = "Customer";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        // Navigation
        public ICollection<Transaction> SalesTransactions { get; set; } = new List<Transaction>();
        public ICollection<PurchaseSalesOrders> PurchaseSalesOrders { get; set; } = new List<PurchaseSalesOrders>();
    
}
}
