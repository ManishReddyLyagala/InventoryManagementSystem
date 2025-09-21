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

        //public List<TransactionDto> Transactions { get; set; } = new();
        public List<PurchaseSalesOrderDto> Orders { get; set; } = new List<PurchaseSalesOrderDto>();
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
}
