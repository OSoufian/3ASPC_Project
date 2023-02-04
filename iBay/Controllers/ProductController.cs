using iBay.Models;
using Microsoft.AspNetCore.Mvc;

namespace iBay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase {

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "AddProduct")]
        public Product Post(){
            return new Product
            {
                Price = 100
            };
        }
        [HttpGet(Name = "GetProduct")]
        public IEnumerable<Models.Product> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Price = 10
            })
            .ToArray();
        }
        [HttpPut(Name = "UpdateProduct")]
        public IEnumerable<Models.Product> Put()
        {
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Price = 10
            })
            .ToArray();
        }
        [HttpDelete(Name = "DeleteProduct")]
        public IEnumerable<Models.Product> Delete()
        {
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Price = 10
            })
            .ToArray();
        }

    }
}