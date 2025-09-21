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
            var customers = await _context.User.Where(u => u.Role == "Customer").ToListAsync();

            return customers.Select(u => new CustomerReadDto
            {
                UserId = u.UserId,
                Name = u.Name,
                MobileNumber = u.MobileNumber,
                EmailID = u.EmailID,
                Role = u.Role
            }).ToList();
        }


        public async Task<CustomerByIDReadDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.User
                                .Include(u => u.PurchaseSalesOrders)
                                    .ThenInclude(po => po.Product)
                                .FirstOrDefaultAsync(u => u.UserId == id && u.Role == "Customer");


            if (customer == null) return null;

            return new CustomerByIDReadDto
            {
                UserId = customer.UserId,
                Name = customer.Name,
                MobileNumber = customer.MobileNumber,
                EmailID = customer.EmailID,
                Role = customer.Role,
                Orders = customer.PurchaseSalesOrders.Select(po => new PurchaseSalesOrderDto
                {
                    OrderId = po.OrderId,
                    TransactionId = po.TransactionId,
                    ProductId = po.ProductId,
                    Quantity = po.Quantity,
                    TotalAmount = po.TotalAmount,
                    OrderType = po.OrderType,     // optional if Transaction exists
                    //SupplierId = po.Transaction?.SupplierId,
                    UserId = po.UserId,
                    SupplierName = po.Product?.Supplier?.Name,
                    Product = po.Product != null ? new ProductCreateDto
                    {
                        ProductId = po.Product.ProductId,
                        Name = po.Product.Name,
                        Category = po.Product.Category,
                        Quantity = po.Product.Quantity
                    } : null
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
            var customer = await _context.User
                                         .FirstOrDefaultAsync(u => u.UserId == id && u.Role == "Customer");

            if (customer == null) return false;

            _context.User.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
