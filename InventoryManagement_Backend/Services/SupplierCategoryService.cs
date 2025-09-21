using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Dtos;
using InventoryManagement.Models;
using InventoryManagement_Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services
{
    public class SupplierCategoryService
    {
        private readonly InventoryDbContext _context;

        public SupplierCategoryService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplierCategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.SupplierCategories
                .Include(sc => sc.Supplier)
                .Select(sc => new SupplierCategoryDto
                {
                    SupplierId = sc.SupplierId,
                    SupplierName = sc.Supplier.Name,
                    ProductCategory = sc.Supplier.ProductCategory,
                    Category = sc.Category.ToString(),
                    LastUpdated = sc.LastUpdated
                })
                .ToListAsync();
        }

        public async Task<SupplierCategoryDto?> GetBySupplierIdAsync(int supplierId)
        {
            var sc = await _context.SupplierCategories
                .Include(sc => sc.Supplier)
                .FirstOrDefaultAsync(sc => sc.SupplierId == supplierId);

            if (sc == null) return null;

            return new SupplierCategoryDto
            {
                SupplierId = sc.SupplierId,
                SupplierName = sc.Supplier.Name,
                ProductCategory = sc.Supplier.ProductCategory,
                Category = sc.Category.ToString(),
                LastUpdated = sc.LastUpdated
            };
        }

        public async Task<bool> UpdateCategoryAsync(UpdateSupplierCategoryDto dto)
        {
            var supplierCategory = await _context.SupplierCategories
                .FirstOrDefaultAsync(sc => sc.SupplierId == dto.SupplierId);

            if (supplierCategory == null)
            {
                // If not exists, create a new record
                var newCategory = new SupplierCategory
                {
                    SupplierId = dto.SupplierId,
                    Category = Enum.Parse<SupplierCategoryType>(dto.Category, true),
                    LastUpdated = DateTime.UtcNow
                };
                _context.SupplierCategories.Add(newCategory);
            }
            else
            {
                supplierCategory.Category = Enum.Parse<SupplierCategoryType>(dto.Category, true);
                supplierCategory.LastUpdated = DateTime.UtcNow;
                _context.SupplierCategories.Update(supplierCategory);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
