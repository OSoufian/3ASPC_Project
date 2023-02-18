using iBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iBay.Controllers {
    [ApiController]
    [Route("cart")]
    public class CartController : ControllerBase {

        private readonly MySQLConnection database;

        public CartController(MySQLConnection database) {
            this.database = database;
        }

        [HttpPost("add/{productId}", Name = "AddCartItem")]
        public async Task<IActionResult> AddCartItem(int productId, int quantity = 1) {
            var product = await database.Product.FindAsync(productId);
            if (product == null) {
                return NotFound("Ce produit est inexistant !");
            }

            //var cart = await GetCartForCurrentUser();
            //if (cart == null) {
            //    return BadRequest();
            //}

            Cart cart = new Cart { UserId = 3 };


            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null) {
                cartItem.Quantity += quantity;
            } else {
                var newItem = new CartItem {
                    ProductId = productId,
                    Quantity = quantity
                };
                cart.Items.Add(newItem);
            }

            await database.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}", Name = "GetCartById")]
        public async Task<IActionResult> GetCartById(int id) {
            var cart = await database.Cart.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null) {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpDelete("{cartId}/items/{itemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartId, int itemId) {
            var cart = await database.Cart.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null) {
                return NotFound();
            }

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null) {
                return NotFound();
            }

            cart.Items.Remove(item);
            await database.SaveChangesAsync();

            return NoContent();
        }
    }
}