namespace InventoryManagement_Backend.Dtos
{
    public class CustomerReadDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Mobile_Number { get; set; }
        public string? Email { get; set; }
    }

    public class CustomerByIDReadDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public long Mobile_Number { get; set; }
        public string? Email { get; set; }

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
