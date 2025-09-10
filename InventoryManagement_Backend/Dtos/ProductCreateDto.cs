namespace InventoryManagement_Backend.Dtos
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; } = 0;
        public string? ImageUrl { get; set; }
        public int SupplierId { get; set; }
    }
}
