using Microsoft.AspNetCore.Mvc;

namespace iBay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {

        private readonly ILogger<CartController> _logger;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }

        /*[HttpPost(Name = "AddCart")]
        public IEnumerable<Cart> Post()
        {
        }
        [HttpGet(Name = "GetCart")]
        public IEnumerable<Cart> Get()
        {
        }
        [HttpPut(Name = "UpdateCart")]
        public IEnumerable<Cart> Put()
        {
        }
        [HttpDelete(Name = "DeleteCart")]
        public IEnumerable<Cart> Delete()
        {
        }*/

    }
}