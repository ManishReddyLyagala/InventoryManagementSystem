using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement_Backend.Models
{
    public enum TransactionStatus
    {
        Pending = "Pending",
        Completed = "Completed",
        Cancelled = "Failed"
    }

    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        /// <summary>
        /// 'P' = Purchase, 'S' = Sale
        /// </summary>
        [Required]
        public string TransactionType { get; set; } = string.Empty;

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        // Navigation: 1 transaction => many PurchaseOrder lines
        public ICollection<PurchaseSalesOrders> PurchaseSalesOrders { get; set; } = new List<PurchaseSalesOrders>();
    }
}
