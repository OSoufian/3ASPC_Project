using iBay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace iBay.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public IActionResult PostImageNews([FromForm] IFormFile file) {
            try {
                if (file == null || file.Length == 0) {
                    return BadRequest("Please send a photo");
                }
                //create unique name for file
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                //set file url
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/news", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create)) {
                    file.CopyTo(stream);
                }

                return Ok(fileName);
            } catch {
                return BadRequest("error in upload image");
            }
        }

        public static async Task<string> PostImage(string apiendpoint, IFormFile data) {
            using (var httpClient = new HttpClient()) {
                var multipartContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(GetFileArray(data));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                multipartContent.Add(fileContent, "file", data.FileName);
                var resultUploadImage = await httpClient.PostAsync(apiendpoint, multipartContent);
                if (resultUploadImage.IsSuccessStatusCode) {
                    var fileName = (await resultUploadImage.Content.ReadAsStringAsync()).Replace("\"", "");
                    return fileName;
                }
                return "";
            }
        }

        public static byte[] GetFileArray(IFormFile file) {
            using (var ms = new MemoryStream()) {
                file.CopyTo(ms);
                return ms.ToArray();
            }
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