using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace InventoryManagement_Backend.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string Mobile_Number { get; set; };

        [MaxLength(50)]
        public string? Email { get; set; }

        // Navigation
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}