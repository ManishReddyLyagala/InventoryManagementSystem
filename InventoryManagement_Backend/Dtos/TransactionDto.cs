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
        public int? CustomerId { get; set; }
      

        //public List<PurchaseSalesOrderDto> PurchaseSalesOrders { get; set; } = new();
    }
}
