namespace InventoryManagement_Backend.Dtos
{
    public class UserReadDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
        public string? Address { get; set; }
    }

    public class UserByIDReadDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string Role { get; set; }
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; }

        //public List<TransactionDto> Transactions { get; set; } = new();
        //public List<PurchaseSalesOrderDto> Orders { get; set; } = new List<PurchaseSalesOrderDto>();
    }

    //public class CreateCustomerDto
    //{
    //    public string Name { get; set; } = string.Empty;
    //    public long Mobile_Number { get; set; }
    //    public string? Email { get; set; }
    //}

    public class UpdateCustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
