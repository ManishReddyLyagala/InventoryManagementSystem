using InventoryManagement_Backend.Models;
using System;

namespace InventoryManagement_Backend.Dtos
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }

        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? SupplierName { get; set; }

        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    }

    public class TransactionCreateDto
    {
        public string TransactionType { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }

        // Optional during creation; defaults to Pending if not provided
        public TransactionStatus? Status { get; set; } = TransactionStatus.Pending;
    }

    public class TransactionUpdateDto
    {
        public string? TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public TransactionStatus? Status { get; set; }
    }
}
