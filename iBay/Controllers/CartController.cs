/*using _3ASPC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3ASPC.Controller
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly MySQLConnection database;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(MySQLConnection database, UserManager<IdentityUser> userManager)
        {
            this.database = database;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the logged-in user
            *//*var user = await _userManager.GetUserAsync(User);*//*

            var cart = await database.Cart.Include(c => c.Products)
                                          .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = user.Id,
                    Products = new List<Product>()
                };
                database.Cart.Add(cart);
            }

            cart.Products.Add(product);

            await database.SaveChangesAsync();

            return Ok(cart);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            // Get the logged-in user
            var user = await _userManager.GetUserAsync(User);

            // Get the cart for the user
            var cart = await database.Cart
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            return Ok($"Your cart : {cart}");
        }
    }
}*/