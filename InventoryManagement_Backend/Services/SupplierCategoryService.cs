using InventoryManagement.Dtos;
using InventoryManagement.Models;
using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Services
{
    public class SupplierCategoryService
    {
        private readonly InventoryDbContext _context;

        public SupplierCategoryService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task UpdateSupplierCategoriesAsync()
        {
            var suppliers = await _context.Suppliers
                .Include(s => s.SupplierOrders)
                .Include(s => s.SupplierCategory)
                .ToListAsync();

            foreach (var supplier in suppliers)
            {
                if (supplier.SupplierOrders == null || supplier.SupplierOrders.Count == 0)
                {
                    SetCategory(supplier, SupplierCategoryType.New);
                    continue;
                }

                int totalOrders = supplier.SupplierOrders.Count;
                int onTimeOrders = supplier.SupplierOrders
                    .Count(o => o.ActualDeliveryDate.Date <= o.ExpectedDeliveryDate.Date);

                
                double onTimeRate = (double)onTimeOrders / totalOrders * 100;
                Console.WriteLine("###############################################################");
                Console.WriteLine(onTimeRate);

                SupplierCategoryType newCategory;
                if (onTimeRate >= 90)
                    newCategory = SupplierCategoryType.Preferred;
                else if (onTimeRate >= 60)
                    newCategory = SupplierCategoryType.Backup;
                else
                    newCategory = SupplierCategoryType.HighRisk;

                SetCategory(supplier, newCategory);
            }

            await _context.SaveChangesAsync();
        }

        private void SetCategory(Supplier supplier, SupplierCategoryType category)
        {
            if (supplier.SupplierCategory == null)
            {
                supplier.SupplierCategory = new SupplierCategory
                {
                    SupplierId = supplier.SupplierId,
                    Category = category,
                    LastUpdated = DateTime.UtcNow
                };
                _context.SupplierCategories.Add(supplier.SupplierCategory);
            }
            else
            {
                supplier.SupplierCategory.Category = category;
                supplier.SupplierCategory.LastUpdated = DateTime.UtcNow;
                _context.SupplierCategories.Update(supplier.SupplierCategory);
            }
        }
    }
}
