namespace InventoryManagement_Backend.Dtos
{
    public class ProductCreateDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; } = 0;
        public string? ImageUrl { get; set; }
        public Decimal Price { get; set; }
        public int SupplierId { get; set; }
    }

    public class product_supplier
    {
        public int ProductId { get; set; }
        public string Product_Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; } = 0;
        public string? ImageUrl { get; set; }
        public Decimal Price { get; set; }
        public int SupplierId { get; set; }
        public string Supplier_Name { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }

        public string S_category { get; set; }

    }
    public class Low_High_Stocks
    {
        public List<ProductCreateDto> Lowstocks { get; set; } = new List<ProductCreateDto>();
        public List<ProductCreateDto> Highstocks { get; set; } = new List<ProductCreateDto>();
    }
}
