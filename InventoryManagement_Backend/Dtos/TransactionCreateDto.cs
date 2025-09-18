namespace InventoryManagement_Backend.Dtos
{
    public class TransactionCreateDto
    {
        public string TransactionType { get; set; } // 'P' or 'S'
        public DateTime TransactionDate { get; set; }

        //public List<PurchaseSalesOrderDto> Lines { get; set; } = new();
    }
}
