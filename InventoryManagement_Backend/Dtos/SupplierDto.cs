using System.ComponentModel.DataAnnotations;

namespace InventoryManagement_Backend.Dtos
{

    public class SupplierReadDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
    }
    // For reading/displaying supplier data
    public class SupplierbyIDReadDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
        public List<ProductReadDto> Products { get; set; } = new();
    }

    public class ProductReadDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        //public int SupplierId { get; set; }
    }

    // For creating a supplier (client should NOT send SupplierId)
    public class SupplierCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
    }

    // For updating a supplier (client sends SupplierId + updatable fields)
    public class SupplierUpdateDto
    {
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
    }
}
