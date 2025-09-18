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
        public string TransactionType { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        // Navigation: 1 transaction => many PurchaseOrder lines
        public ICollection<PurchaseSalesOrders> PurchaseSalesOrders { get; set; } = new List<PurchaseSalesOrders>();
    }
}
