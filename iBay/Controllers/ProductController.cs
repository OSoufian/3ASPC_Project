using iBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iBay.Controllers {
    //[Authorize]
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase {

        private readonly MySQLConnection database;

        public ProductController(MySQLConnection database) {
            this.database = database;
        }

        [HttpPost(Name = "AddProduct")]
        public async Task<IActionResult> Post(Product product) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

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
        public async Task<IActionResult> Get() {
            List<Product> List = await database.Product.Select(
                s => new Product {
                    Id = s.Id,
                    Price = s.Price,
                    Name = s.Name,
                    Available = s.Available,
                    Added_Time = s.Added_Time,
                    Image = s.Image
                }
            ).ToListAsync();

            if (List.Count < 0) {
                return NotFound("ouups");
            } else {
                return Ok(List);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int Id) {
            if (Id == 0) return BadRequest();
            Product Product = await database.Product.Select(
                    s => new Product {
                        Id = s.Id,
                        Price = s.Price,
                        Name = s.Name,
                        Available = s.Available,
                        Added_Time = s.Added_Time,
                        Image = s.Image,
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (Product == null) {
                return NotFound("Pas de produit");
            } else {
                return Ok(Product);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Product product, int Id) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            Product updatedProduct = await database.Product.FirstOrDefaultAsync(s => s.Id == Id);
            Product defaultProduct = new Product();

            updatedProduct.Price = product.Price == defaultProduct.Price ? updatedProduct.Price : product.Price;
            updatedProduct.Name = product.Name == defaultProduct.Name ? updatedProduct.Name : product.Name;
            updatedProduct.Available = product.Available == defaultProduct.Available ? updatedProduct.Available : product.Available;
            updatedProduct.Added_Time = product.Added_Time == defaultProduct.Added_Time ? updatedProduct.Added_Time : product.Added_Time;
            updatedProduct.Image = product.Image == null ? updatedProduct.Image : product.Image;

            await database.SaveChangesAsync();
            return Ok(updatedProduct);
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