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
        public List<ProductCreateDto> Products { get; set; } = new();
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
