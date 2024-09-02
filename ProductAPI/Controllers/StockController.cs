using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
        }

        [HttpGet("getdata")]
        public IActionResult Getdata()
        {
            try
            {
                var products = _context.Product.ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {

                return BadRequest("errer" + ex.Message);
            }
        }


        [HttpPost("update")]
        public IActionResult Update([FromBody] List<Product> products)
        {
            if (products == null || !products.Any())
            {
                return BadRequest("product no data");
            }

            foreach (var prod in products)
            {
                var existingProduct = _context.Product.FirstOrDefault(p => p.Id == prod.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = prod.Name;
                    existingProduct.Price = prod.Price;
                    existingProduct.Quantity = prod.Quantity;
                    _context.Product.Update(existingProduct);
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"error: {ex.Message}");
            }

            return Ok("Products updated successfully");
        }

    }
}
