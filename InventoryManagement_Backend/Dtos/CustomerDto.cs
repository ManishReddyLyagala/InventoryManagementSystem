namespace InventoryManagement_Backend.Dtos
{
    public class CustomerReadDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
    }

    public class CustomerByIDReadDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";

        public List<TransactionDto> Transactions { get; set; } = new();
    }

    //public class CreateCustomerDto
    //{
    //    public string Name { get; set; } = string.Empty;
    //    public long Mobile_Number { get; set; }
    //    public string? Email { get; set; }
    //}

    //public class UpdateCustomerDto
    //{
    //    public string Name { get; set; } = string.Empty;
    //    public long Mobile_Number { get; set; }
    //    public string? Email { get; set; }
    //}

    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public int? SupplierId { get; set; }
        public int? CustomerId { get; set; }
        public List<PurchaseSalesOrderDto> Orders { get; set; } = new();
    }
}
