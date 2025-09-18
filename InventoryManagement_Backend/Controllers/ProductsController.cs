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
            try
            {
                var products = await _productservice.get_all_products();
                if (products == null || !products.Any())
                {
                    return NotFound("No products found");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> addproduct([FromBody] ProductCreateDto new_product)
        {
            try
            {
                if (new_product == null)
                    return BadRequest("Product cannot be null");

                if (new_product.ProductId < 0)
                    return BadRequest("ProductId cannot be negative");

                if (string.IsNullOrWhiteSpace(new_product.Name))
                    return BadRequest("Product name is required");

                if (string.IsNullOrWhiteSpace(new_product.Category))
                    return BadRequest("Category is required");

                if (string.IsNullOrWhiteSpace(new_product.Description))
                    return BadRequest("Description is required");

                if (new_product.Quantity < 0)
                    return BadRequest("Quantity cannot be negative");

                if (!string.IsNullOrWhiteSpace(new_product.ImageUrl) &&
                    !Uri.IsWellFormedUriString(new_product.ImageUrl, UriKind.Absolute))
                {
                    return BadRequest("ImageUrl must be a valid URL");
                }

                if (new_product.Price <= 0)
                    return BadRequest("Price must be greater than 0");

                if (new_product.SupplierId <= 0)
                    return BadRequest("SupplierId is required and must be positive");

                int supplier_exist = await _productservice.add_product(new_product);
                if (supplier_exist == -1)
                {
                    return NotFound("Supplier Id does not exist");
                }

                return CreatedAtAction(nameof(get_productby_id), new { id = supplier_exist }, "Added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<product_supplier>> get_productby_id(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid product id");

                var product_supplier = await _productservice.get_productby_id(id);
                if (product_supplier == null)
                {
                    return NotFound("Product not found");
                }
                return Ok(product_supplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/quantity")]
        public async Task<ActionResult<int>> get_qunatity(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid product id");

                var quantity = await _productservice.get_quantity(id);
                if (quantity == -1) return NotFound("Product does not exist");
                return Ok(quantity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> del_product(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid product id");

                bool b = await _productservice.del_product(id);
                if (b)
                {
                    return Ok("Deleted successfully");
                }
                else { return NotFound("Product Id does not exist"); }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id}/update_qunatity")]
        public async Task<ActionResult<bool>> update_quantity(int id, int inc_dec, [FromQuery] int threshold=10)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid product id");

                bool b = await _productservice.update_quantity(id, inc_dec, threshold);
                if (b) return Ok(true);
                return NotFound("Quantity can't go below zero");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductCreateDto>> update_product(int id, [FromBody] ProductCreateDto new_product)
        {
            try
            {
                if (new_product == null)
                    return BadRequest("Product cannot be null");

                if (new_product.ProductId < 0)
                    return BadRequest("ProductId cannot be negative");

                if (string.IsNullOrWhiteSpace(new_product.Name))
                    return BadRequest("Product name is required");

                if (string.IsNullOrWhiteSpace(new_product.Category))
                    return BadRequest("Category is required");

                if (string.IsNullOrWhiteSpace(new_product.Description))
                    return BadRequest("Description is required");

                if (new_product.Quantity < 0)
                    return BadRequest("Quantity cannot be negative");

                if (!string.IsNullOrWhiteSpace(new_product.ImageUrl) &&
                    !Uri.IsWellFormedUriString(new_product.ImageUrl, UriKind.Absolute))
                {
                    return BadRequest("ImageUrl must be a valid URL");
                }

                if (new_product.Price <= 0)
                    return BadRequest("Price must be greater than 0");

                if (new_product.SupplierId <= 0)
                    return BadRequest("SupplierId is required and must be positive");

                bool b = await _productservice.edit_product(id, new_product);
                if (b)
                    return Ok(new_product);
                else
                    return BadRequest("Id cannot be changed or product not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPatch("{id}/change_supplier")]
        public async Task<ActionResult<string>> change_supplier(int id, int supplier_id)
        {
            try
            {
                if (id <= 0 || supplier_id <= 0)
                    return BadRequest("Invalid product/supplier id");

                bool b = await _productservice.edit_supplier(id, supplier_id);
                if (b) return Ok("Supplier changed successfully");
                else return NotFound("Incorrect SupplierId or ProductId");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get_stocks")]
        public async Task<ActionResult<Low_High_Stocks>> get_stocks([FromQuery] int threshold = 10)
        {
            try
            {
                var result = await _productservice.segreating_low_high_stocks(threshold);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{type}/filter_products")]
        public async Task<ActionResult<IList<ProductCreateDto>>> filter_products_type(string type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(type))
                    return BadRequest("Filter type is required");

                var filter_products = await _productservice.filter(type);
                if (filter_products == null || !filter_products.Any())
                    return NotFound("No products found");
                return Ok(filter_products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IList<ProductCreateDto>>> Search(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("Search term cannot be empty");

                var products = await _productservice.search(name);
                if (products == null || !products.Any())
                    return NotFound("No products found");
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 5)
        {
            try
            {
                if (page <= 0) return BadRequest("Page must be greater than 0");
                if (pageSize <= 0) return BadRequest("PageSize must be greater than 0");

                var result = await _productservice.GetPagedProducts(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
