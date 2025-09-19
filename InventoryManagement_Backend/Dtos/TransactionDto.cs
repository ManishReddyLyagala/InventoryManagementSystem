namespace InventoryManagement_Backend.Dtos
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }


        public int OrderId { get; set; }
    
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
  
        public int? SupplierId { get; set; }
        public int? UserId { get; set; }
      

        //public List<PurchaseSalesOrderDto> PurchaseSalesOrders { get; set; } = new();
    }

    public class TransactionCreateDto
    {
        public string TransactionType { get; set; } // 'P' or 'S'
        public DateTime TransactionDate { get; set; }

        //public List<PurchaseSalesOrderDto> Lines { get; set; } = new();
    }
}
