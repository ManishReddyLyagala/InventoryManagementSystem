using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement_Backend.Models;
using InventoryManagement_Backend.Services;
using InventoryManagement_Backend.Dtos;

namespace InventoryManagement_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productservice;
        public ProductsController(IProductService productservices)
        {
            _productservice = productservices;
        }
        [HttpGet]
        public async Task<ActionResult<IList<ProductCreateDto>>> getproducts()
        {
            var products = await _productservice.get_all_products();
            if (products == null)
            {
                return NotFound("NO Products");
            }
            return Ok(products);
        }
        [HttpPost]
        public async Task<ActionResult<string>> addproduct(ProductCreateDto new_product)
        {
            int supplier_exist = await _productservice.add_product(new_product);
            if (supplier_exist == -1)
            {
                return NotFound("Supplier Id does not exists");
            }
            return CreatedAtAction(nameof(get_productby_id), new { id = supplier_exist }, "Added successfully");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<product_supplier>> get_productby_id(int id)
        {
            var product_supplier = await _productservice.get_productby_id(id);
            if (product_supplier == null)
            {
                return NotFound("Product not found");
            }
            return Ok(product_supplier);
        }

        [HttpGet("{id}/quantity")]
        public async Task<ActionResult<int>> get_qunatity(int id)
        {
            var quantity = await _productservice.get_quantity(id);
            if (quantity == -1) return NotFound("product doesnot exists");
            return Ok(quantity);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> del_product(int id)
        {
            bool b = await _productservice.del_product(id);
            if (b)
            {
                return "Deleted sucessfully";
            }
            else { return NotFound("Product Id doesnot exists"); }
        }

        [HttpPatch("{id}/update_qunatity")]
        public async Task<ActionResult<bool>> update_quantity(int id, int inc_dec)
        {
            bool b = await _productservice.update_quantity(id, inc_dec);
            if (b) return CreatedAtAction(nameof(get_productby_id), new { id = id }, true); ;
            return NotFound("Quantity can't go below zero");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductCreateDto>> update_product(int id, ProductCreateDto new_product)
        {
            bool b = await _productservice.edit_product(id, new_product);
            if (b) return CreatedAtAction(nameof(get_productby_id), new { id = new_product.ProductId }, new_product);
            else return BadRequest("Id cannot be changed");
        }

        [HttpPatch("{id}/change_supplier")]
        public async Task<ActionResult<string>> change_supplier(int id, int supplier_id)
        {
            bool b = await _productservice.edit_supplier(id, supplier_id);
            if (b) return CreatedAtAction(nameof(get_productby_id), new { id = id }, "Supplier Changed");
            else return NotFound("Incorrect Supplier_data/Product_id");
        }

        [HttpGet("get_stocks")]
        public async Task<ActionResult<Low_High_Stocks>> get_stocks()
        {
            return await _productservice.segreating_low_high_stocks();
        }
        [HttpGet("{type}/filter_products")]
        public async Task<ActionResult<IList<ProductCreateDto>>> filter_products_type(string type)
        {
            var filter_products = await _productservice.filter(type);
            if (filter_products == null) return NotFound("No products found");
            return Ok(filter_products);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IList<ProductCreateDto>>> Search(string name)
        {
            var products = await _productservice.search(name);
            if (products == null) return NotFound("No products found");
            return Ok(products);
        }
        [HttpGet("pagination")]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 5)
        {
            var result = await _productservice.GetPagedProducts(page, pageSize);
            return Ok(result);
        }
    }
}
