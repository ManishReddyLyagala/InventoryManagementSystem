namespace InventoryManagement_Backend.Dtos
{
    public class PurchaseOrderCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
