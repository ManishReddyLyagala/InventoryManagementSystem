using InventoryManagement_Backend.Data;
using InventoryManagement_Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagement_Backend.Dtos;

namespace InventoryManagement_Backend.Services
{
    public class ProductService : IProductService
    {

        private readonly InventoryDbContext _context;
        private readonly IStockAlertService _stockAlertService;
        public ProductService(InventoryDbContext context, IStockAlertService stockAlertService)
        {
            _context = context;
            _stockAlertService = stockAlertService;
        }
        public async Task<IList<ProductCreateDto>> get_all_products()
        {
            return await _context.Products.Select(p => new ProductCreateDto { ProductId = p.ProductId, Name = p.Name, Category = p.Category, Description = p.Description, Quantity = p.Quantity, ImageUrl = p.ImageUrl, Price = p.Price, SupplierId = p.SupplierId }).ToListAsync();
        }

        public async Task<int> add_product(ProductCreateDto new_product)
        {
            var supplier_exist = await _context.Suppliers.FirstOrDefaultAsync(p => p.SupplierId == new_product.SupplierId);
            if (supplier_exist == null)
            {
                return -1;
            }
            Product product = new Product
            {
                Name = new_product.Name,
                Category = new_product.Category,
                Description = new_product.Description,
                Quantity = new_product.Quantity,
                Price = new_product.Price,
                ImageUrl = new_product.ImageUrl,
                SupplierId = new_product.SupplierId,
                Supplier = supplier_exist
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<product_supplier> get_productby_id(int id)
        {
            var product = await _context.Products.Include(p => p.Supplier).FirstOrDefaultAsync(x => x.ProductId == id);
            //var product= await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) { return null; }
            product_supplier p_s = new product_supplier
            {
                ProductId = product.ProductId,
                Product_Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Quantity = product.Quantity,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                SupplierId = product.SupplierId,
                Supplier_Name = product.Supplier.Name,
                MobileNumber = product.Supplier.MobileNumber,
                EmailId = product.Supplier.EmailID
            };
            return p_s;
        }

        public async Task<bool> del_product(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return false;
            }
            else
            {
                _context.Products.Remove(product);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"##################################################################Internal server error: {ex.Message}");
                }
                return true;
            }
        }

        public async Task<int> get_quantity(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return -1;
            return product.Quantity;
        }

        public async Task<bool> update_quantity(int id, int inc_dec, int threshold)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return false;

            int quantity = product.Quantity + inc_dec; // use inc_dec directly
            Console.WriteLine($"Quantity: {quantity}, inc_dec: {inc_dec}");

            if (quantity < 0) return false;

            product.Quantity = quantity;
            await _context.SaveChangesAsync();

            if (product.Quantity < threshold)
            {
                await _stockAlertService.SendDailyLowStockEmailAsync(threshold);
            }

            return true;
        }


        public async Task<bool> edit_product(int id, ProductCreateDto new_product)
        {
            //if(id!=new_product.ProductId) return false;
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (product == null) return false;
            product.Name = new_product.Name;
            product.Description = new_product.Description;
            product.Category = new_product.Category;
            product.ImageUrl = new_product.ImageUrl;
            product.Quantity = new_product.Quantity;
            product.Price = new_product.Price;
            if (product.SupplierId != new_product.SupplierId)
            {
                var supplier_exist = await _context.Products.Include(p => p.Supplier).FirstOrDefaultAsync(x => x.SupplierId == new_product.SupplierId);
                if (supplier_exist != null)
                {
                    product.SupplierId = new_product.SupplierId;
                }
                else
                {
                    return false;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> edit_supplier(int product_id, int supplier_id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == product_id);
            if (product == null) return false;
            var supplier_exist = await _context.Suppliers.FirstOrDefaultAsync(p => p.SupplierId == supplier_id);
            if (supplier_exist == null) return false;
            product.Supplier = supplier_exist;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Low_High_Stocks> segreating_low_high_stocks(int threshold)
        {
            var products = await _context.Products.ToListAsync();

            Low_High_Stocks ls = new Low_High_Stocks();

            foreach (var product in products)
            {
                ProductCreateDto dto = map(product);

                if (product.Quantity > threshold) ls.Highstocks.Add(dto);
                else ls.Lowstocks.Add(dto);
            }
            return ls;
        }
        public async Task<IList<ProductCreateDto>> filter(string category)
        {
            var products = await _context.Products.Where(p => p.Category.ToLower().Trim() == category.ToLower().Trim()).ToListAsync();
            if (products == null) return null;
            List<ProductCreateDto> products_filtered = new List<ProductCreateDto>();
            foreach (var product in products)
            {
                Console.WriteLine(product.Name);
                products_filtered.Add(map(product));
            }
            return products_filtered;
        }
        public ProductCreateDto map(Product product)
        {
            ProductCreateDto p = new ProductCreateDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Category = product.Category,
                SupplierId = product.SupplierId,
                Quantity = product.Quantity,
            };
            return p;
        }
        public async Task<IList<ProductCreateDto>> search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return null;

            keyword = keyword.ToLower().Trim();

            var products = await _context.Products
                .Where(p =>
                    p.Name.ToLower().Contains(keyword) ||
                    (!string.IsNullOrEmpty(p.Category) && p.Category.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(keyword))
                )
                .ToListAsync();

            if (products.Count == 0) return null;

            return products.Select(p => map(p)).ToList();
        }

        public async Task<object> GetPagedProducts(int page, int pageSize = 5)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 5;

            var totalItems = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var products = await _context.Products
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtoProducts = products.Select(p => map(p)).ToList();

            return new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Items = dtoProducts
            };
        }

        public async Task<product_supplier> get_product_supplier(int id)
        {
            var product = await _context.Products.Include(p => p.Supplier).FirstOrDefaultAsync(x => x.ProductId == id);
            if (product == null) return null;
            product_supplier p = new product_supplier
            {
                ProductId = product.ProductId,
                Product_Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = product.Quantity,
                SupplierId = product.SupplierId,
                Supplier_Name = product.Supplier.Name,
                MobileNumber = product.Supplier.MobileNumber,
                EmailId = product.Supplier.EmailID,
                S_category = product.Supplier.ProductCategory
            };
            return p;
        }

        public async Task<IList<product_supplier>> get_all_product_supplier()
        {
            var products = await _context.Products.Include(p => p.Supplier).ToListAsync();
            if (products == null) return null;
            List<product_supplier> p_s = new List<product_supplier>();
            foreach (var product in products)
            {
                product_supplier p = new product_supplier
                {
                    ProductId = product.ProductId,
                    Product_Name = product.Name,
                    Category = product.Category,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    SupplierId = product.SupplierId,
                    Supplier_Name = product.Supplier.Name,
                    MobileNumber = product.Supplier.MobileNumber,
                    EmailId = product.Supplier.EmailID,
                    S_category = product.Supplier.ProductCategory
                };
                p_s.Add(p);
            }
            return p_s;
        }
    }
}
