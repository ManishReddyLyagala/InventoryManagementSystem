using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using Microsoft.EntityFrameworkCore;
using InventoryManagement_Backend.Data;

namespace InventoryManagement_Backend.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly InventoryDbContext _context;

        public SupplierService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplierReadDto>> GetAllAsync()
        {
            return await _context.Suppliers
                .Include(s => s.Products)
                .Select(s => new SupplierReadDto
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    MobileNumber = s.MobileNumber,
                    EmailID = s.EmailID,
                    ProductCategory = s.ProductCategory,
                    //Products = s.Products.Select(p => new ProductReadDto
                    //{
                    //    ProductId = p.ProductId,
                    //    Name = p.Name,
                    //    Category = p.Category ?? string.Empty,
                    //    Description = p.Description ?? string.Empty,
                    //    Quantity = p.Quantity,
                    //    ImageUrl = p.ImageUrl ?? string.Empty
                    //}).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<SupplierbyIDReadDto?> GetByIdAsync(int id)
        {
            return await _context.Suppliers
                .Where(s => s.SupplierId == id)
                .Include(s => s.Products)  // eager load products
                .Select(s => new SupplierbyIDReadDto
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name,
                    MobileNumber = s.MobileNumber,
                    EmailID = s.EmailID,
                    ProductCategory = s.ProductCategory,
                    Products = s.Products.Select(p => new ProductCreateDto
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Category = p.Category,
                        Description = p.Description,
                        Quantity = p.Quantity,
                        ImageUrl = p.ImageUrl,
                        //SupplierId = p.SupplierId
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


        public async Task<SupplierReadDto> CreateAsync(SupplierCreateDto dto)
        {
            var supplier = new Supplier
            {
                Name = dto.Name,
                MobileNumber = dto.MobileNumber,
                EmailID = dto.EmailID,
                ProductCategory = dto.ProductCategory
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return new SupplierReadDto
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                MobileNumber = supplier.MobileNumber,
                EmailID = supplier.EmailID,
                ProductCategory = supplier.ProductCategory
            };
        }

        public async Task<bool> PatchAsync(int id, SupplierUpdateDto dto)
        {
            if (id != dto.SupplierId) return false;

            var existing = await _context.Suppliers.FindAsync(id);
            if (existing == null) return false;

            if (!string.IsNullOrEmpty(dto.Name))
                existing.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.EmailID))
                existing.MobileNumber = dto.MobileNumber; 

            if (!string.IsNullOrEmpty(dto.EmailID))
                existing.EmailID = dto.EmailID;

            if (!string.IsNullOrEmpty(dto.ProductCategory))
                existing.ProductCategory = dto.ProductCategory;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return false;

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
