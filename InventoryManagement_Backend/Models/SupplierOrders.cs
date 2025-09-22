using InventoryManagement.Models;
using InventoryManagement_Backend.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public class SupplierOrder
    {
        [Key]
        public int SupplierOrderId { get; set; }

        [ForeignKey("PurchaseSalesOrder")]
        public int OrderId { get; set; }
        public PurchaseSalesOrders PurchaseSalesOrder { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime ActualDeliveryDate { get; set; }
    }
}

