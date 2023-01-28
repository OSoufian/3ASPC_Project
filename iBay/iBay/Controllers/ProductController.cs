using Microsoft.AspNetCore.Mvc;

namespace iBay.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase {

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger) {
            _logger = logger;
        }

        [HttpPost(Name = "AddProduct")]
        public IEnumerable<Product> Post()
        {
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Price = 10
            })
            .ToArray();
        }
        [HttpGet(Name = "GetProduct")]
        public IEnumerable<Product> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Price = 10
            })
            .ToArray();
        }
        [HttpPut(Name = "UpdateProduct")]
        public IEnumerable<Product> Put()
        {
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Price = 10
            })
            .ToArray();
        }
        [HttpDelete(Name = "DeleteProduct")]
        public IEnumerable<Product> Delete()
        {
            return Enumerable.Range(1, 5).Select(index => new Product
            {
                Price = 10
            })
            .ToArray();
        }

    }
}