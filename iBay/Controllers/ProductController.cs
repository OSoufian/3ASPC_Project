using iBay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace iBay.Controllers {
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase {

        private readonly ILogger<ProductController> _logger;
        private readonly MySQLConnection database;

        public ProductController(ILogger<ProductController> logger, MySQLConnection database) {
            this.database = database;
            _logger = logger;
        }

        [HttpPost(Name = "AddProduct")]
        public async Task<IActionResult> Post(Product product) {
            Product newProduct = new Product() {
                Price = product.Price,
                Name = product.Name,
                Available = product.Available,
                Added_Time = product.Added_Time,
                Image = product.Image
            };

            database.Add(newProduct);
            await database.SaveChangesAsync();

            return Created($"Product/{newProduct.Id}", newProduct);
        }

        [HttpGet()]
        public async Task<ActionResult<List<Product>>> Get() {
            var List = await database.Product.Select(
                s => new Product {
                    Id = s.Id,
                    Price = s.Price,
                    Available = s.Available,
                    Added_Time = s.Added_Time,
                    Image = s.Image
                }
            ).ToListAsync();

            if (List.Count < 0) {
                return NotFound("ouups");
            } else {
                return List;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int Id) {
            Product Product = await database.Product.Select(
                    s => new Product {
                        Id = s.Id,
                        Price = s.Price,
                        Available = s.Available,
                        Added_Time = s.Added_Time,
                        Image = s.Image,
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (Product == null) {
                return NotFound("Pas d'utilisateur");
            } else {
                return Product;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Product product, int Id) {
            Product updatedProduct = await database.Product.FirstOrDefaultAsync(s => s.Id == Id);

            updatedProduct.Price = product.Price == 10000000000000 ? updatedProduct.Price : product.Price;
            updatedProduct.Available = product.Available != updatedProduct.Available ? updatedProduct.Available : product.Available;
            updatedProduct.Added_Time = product.Added_Time == new DateTime(2017, 8, 24) ? updatedProduct.Added_Time : product.Added_Time;
            updatedProduct.Image = product.Image == null ? updatedProduct.Image : product.Image;

            await database.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int Id) {
            Product Product = new Product() {
                Id = Id
            };

            database.Product.Attach(Product);
            database.Product.Remove(Product);
            await database.SaveChangesAsync();
            return NoContent();
        }

    }
}