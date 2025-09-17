using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement_Backend.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly InventoryDbContext _context;

        public CustomerService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerReadDto>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Select(c => new CustomerReadDto
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Mobile_Number = c.Mobile_Number,
                    Email = c.Email
                })
                .ToListAsync();
        }

        //public async Task<CustomerDetailDto?> GetCustomerByIdAsync(int id)
        //{
        //    var customer = await _context.Customers
        //        .Include(c => c.Transactions)
        //            .ThenInclude(t => t.PurchaseOrders)
        //             .ThenInclude(p => p.Product)
        //        .FirstOrDefaultAsync(c => c.CustomerId == id);

        //    if (customer == null) return null;

        //    return new CustomerDetailDto
        //    {
        //        CustomerId = customer.CustomerId,
        //        Name = customer.Name,
        //        Mobile_Number = customer.Mobile_Number,
        //        Email = customer.Email,
        //        Orders = customer.Transactions.Select(t => new TransactionDto
        //        {
        //            TransactionId = t.TransactionId,
        //            Type = t.Type,
        //            DateTime = t.DateTime,
        //            SupplierId = t.SupplierId,
        //            CustomerId = t.CustomerId
        //        }).ToList()
        //    };
        //}

        public async Task<CustomerByIDReadDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Transactions)
                    .ThenInclude(t => t.PurchaseOrders)
                        .ThenInclude(po => po.Product)
                            .ThenInclude(t => t.Supplier)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null) return null;

            return new CustomerByIDReadDto
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Mobile_Number = customer.Mobile_Number,
                Email = customer.Email,
                Transactions = customer.Transactions.Select(t => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    Type = t.Type,
                    DateTime = t.DateTime,
                    SupplierId = t.SupplierId,

                    Orders = t.PurchaseOrders.Select(po => new PurchaseSalesOrderDto
                    {
                        OrderId = po.OrderId,
                        TransactionId = po.TransactionId,
                        ProductId = po.ProductId,
                        Quantity = po.Quantity,
                        TotalAmount = po.TotalAmount,
                        OrderType = t.Type,
                        SupplierId = t.SupplierId,
                        CustomerId = t.CustomerId,
                        OrderDate = t.DateTime,

                        SupplierName = po.Product.SupplierId != null ? po.Product.Supplier.Name : null,

                        Product = new ProductReadDto
                        {
                            ProductId = po.Product.ProductId,
                            Name = po.Product.Name,
                            Category = po.Product.Category,
                            Quantity = po.Product.Quantity
                        }
                    }).ToList()
                }).ToList()
            };
        }


        //public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
        //{
        //    var customer = new Customer
        //    {
        //        Name = dto.Name,
        //        Mobile_Number = dto.Mobile_Number,
        //        Email = dto.Email
        //    };

        //    _context.Customers.Add(customer);
        //    await _context.SaveChangesAsync();

        //    return new CustomerDto
        //    {
        //        CustomerId = customer.CustomerId,
        //        Name = customer.Name,
        //        Mobile_Number = customer.Mobile_Number,
        //        Email = customer.Email
        //    };
        //}

        //public async Task<bool> UpdateCustomerAsync(int id, UpdateCustomerDto dto)
        //{
        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer == null) return false;

        //    customer.Name = dto.Name;
        //    customer.Mobile_Number = dto.Mobile_Number;
        //    customer.Email = dto.Email;

        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
