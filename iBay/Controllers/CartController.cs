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

        [HttpPost("{productId}", Name = "AddCartItem")]
        public async Task<IActionResult> AddCartItem(int productId, int quantity = 1) {
            var product = await database.Product.FindAsync(productId);
            if (product == null) {
                return NotFound("Ce produit est inexistant !");
            }

            //var cart = await GetCartForCurrentUser();
            //if (cart == null) {
            //    return BadRequest();
            //}

            // TODO: Remplacer User_id par user connecté

            Cart cart = new Cart { User_Id = 3 };
            database.Cart.Add(cart);


            //var cart_Item = cart.Items.FirstOrDefault(item => item.Product_Id == productId);

            var cart_Item = await database.Cart_Item.Select(
                    s => new CartItem {
                        Id = s.Id,
                        Price = s.Price,
                        CartId = s.CartId,
                        Product_Id = s.Product_Id,
                        Quantity = s.Quantity
                    })
                .FirstOrDefaultAsync(s => s.Product_Id == productId);


            if (cart_Item != null) {
                cart_Item.Quantity += quantity;
                database.Update(cart_Item);
            } else {
                var newItem = new CartItem {
                    Product_Id = productId,
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