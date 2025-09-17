namespace InventoryManagement_Backend.Dtos
{
    public class TransactionCreateDto
    {
        public char Type { get; set; } // 'P' or 'S'
        public DateTime? DateTime { get; set; }

        // specify one of SupplierId / CustomerId depending on type
        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }

        public List<PurchaseSalesOrderDto> Orders { get; set; } = new();
    }
}