using iBay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace iBay.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase {

        private readonly ILogger<ProductController> _logger;
        private readonly MySQLConnection database;

        public ProductController(ILogger<ProductController> logger, MySQLConnection database)
        {
            this.database = database;
            _logger = logger;
        }

        //[HttpPost(Name = "AddProduct")]
        //public IActionResult Post([FromForm]Product product)
        //{
        //    try {
        //        if (product.Image == null || product.Image.Length == 0) {
        //            return BadRequest("Please send a photo");
        //        }
        //        //create unique name for file
        //        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.Image.FileName);

        //        //set file url
        //        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/", fileName);

        //        using (var stream = new FileStream(savePath, FileMode.Create)) {
        //            product.Image.CopyTo(stream);
        //        }

        //        return Ok(fileName);
        //    } catch {
        //        return BadRequest("error in upload image");
        //    }
        //}

        //TODO : faire que ça marche
        [HttpPost(Name = "AddProduct")]
        public async Task<IActionResult> Index(List<IFormFile> files) {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files) {
                if (formFile.Length > 0) {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create)) {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, filePaths });
        }

        [HttpGet()]
        public async Task<ActionResult<List<Product>>> Get()
        {
            var List = await database.Product.Select(
                s => new Product
                {
                    Id = s.Id,
                    Price = s.Price,
                    Available = s.Available,
                    Added_Time = s.Added_Time,
                    Image = s.Image
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound("ouups");
            }
            else
            {
                return List;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int Id)
        {
            Product Product = await database.Product.Select(
                    s => new Product
                    {
                        Id = s.Id,
                        Price = s.Price,
                        Available = s.Available,
                        Added_Time = s.Added_Time,
                        Image = s.Image,
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (Product == null)
            {
                return NotFound("Pas d'utilisateur");
            }
            else
            {
                return Product;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Product product, int Id)
        {
            Product updatedProduct = await database.Product.FirstOrDefaultAsync(s => s.Id == Id);

            updatedProduct.Price = product.Price == 10000000000000 ? updatedProduct.Price : product.Price;
            updatedProduct.Available = product.Available != updatedProduct.Available ? updatedProduct.Available : product.Available;
            updatedProduct.Added_Time = product.Added_Time == new DateTime(2017, 8, 24) ? updatedProduct.Added_Time : product.Added_Time;
            updatedProduct.Image = product.Image == null ? updatedProduct.Image : product.Image;

            await database.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            Product Product = new Product()
            {
                Id = Id
            };

            database.Product.Attach(Product);
            database.Product.Remove(Product);
            await database.SaveChangesAsync();
            return NoContent();
        }

    }
}