namespace InventoryManagement_Backend.Dtos
{
    public class TransactionCreateDto
    {
        public int TransactionId { get; set; }
        public char Type { get; set; } // 'P' or 'S'
        public DateTime? DateTime { get; set; }

        // specify one of SupplierId / CustomerId depending on type
        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }

        public List<PurchaseOrderCreate> Lines { get; set; } = new();
    }
}
