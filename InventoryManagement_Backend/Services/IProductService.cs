using InventoryManagement_Backend.Dtos;
using InventoryManagement_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement_Backend.Services
{
    public interface IProductService
    {
        Task<IList<ProductCreateDto>> get_all_products();
        Task<int> add_product(ProductCreateDto new_product);
        Task<product_supplier> get_productby_id(int id);
        Task<bool> del_product(int id);
        Task<int> get_quantity(int id);
        Task<bool> update_quantity(int id, int inc_dec);
        Task<bool> edit_product(int id, ProductCreateDto new_product);
        Task<bool> edit_supplier(int product_id, int supplier_id);
        Task<Low_High_Stocks> segreating_low_high_stocks();
        Task<IList<ProductCreateDto>> filter(string category);
        Task<IList<ProductCreateDto>> search(string keyword);
        Task<object> GetPagedProducts(int page, int pageSize = 5);
    }
}
