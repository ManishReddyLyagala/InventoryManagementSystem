using InventoryManagement_Backend.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement_Backend.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        /// <summary>
        /// 'P' = Purchase, 'S' = Sale
        /// </summary>
        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        // Nullable depending on Type
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // Navigation: 1 transaction => many PurchaseOrder lines
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
