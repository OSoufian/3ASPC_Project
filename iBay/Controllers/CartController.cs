using iBay.Models;
using iBay.Responses;
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

            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue) {
                UserResponse user = await database.User.Select(
                s => new UserResponse {
                    Id = s.Id,
                    Email = s.Email,
                    Pseudo = s.Pseudo,
                    Role = s.Role
                }
            ).FirstOrDefaultAsync(s => s.Id == userId);
            } else {
                return Unauthorized("Aucun utilisateur connecté !");
            }


            var cart = await database.Cart.FirstOrDefaultAsync(c => c.User_Id == userId);
            if (cart == null) {
                cart = new Cart { User_Id = (int)userId };
                database.Cart.Add(cart);
            }

            await database.SaveChangesAsync();

            var cart_Item = await database.Cart_Item.Select(
                    s => new CartItem {
                        Id = s.Id,
                        Price = product.Price,
                        CartId = s.CartId,
                        Product_Id = s.Product_Id,
                        Quantity = s.Quantity
                    })
                .FirstOrDefaultAsync(s => s.Product_Id == productId);


            if (cart_Item != null) {
                cart_Item.Quantity += quantity;
                cart.Total_Price += cart_Item.Price;
                database.Update(cart_Item);
                database.Update(cart);
            } else {
                var newItem = new CartItem {
                    Product_Id = productId,
                    Quantity = quantity,
                    Price = product.Price
                };
                cart.Total_Price += newItem.Price;
                cart.Items.Add(newItem);
                database.Update(cart);
            }



            await database.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("MyCart", Name = "GetCart")]
        public async Task<IActionResult> GetCart() {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue) {
                UserResponse user = await database.User.Select(
                s => new UserResponse {
                    Id = s.Id,
                    Email = s.Email,
                    Pseudo = s.Pseudo,
                    Role = s.Role
                }
            ).FirstOrDefaultAsync(s => s.Id == userId);
            } else {
                return Unauthorized("Aucun utilisateur connecté !");
            }


            var cart = await database.Cart.FirstOrDefaultAsync(c => c.User_Id == userId);
            if (cart == null) {
                return NotFound("Vous n'avez acheté encore aucun article !");
            }

            return Ok(cart);

        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> RemoveCartItem(int itemId) {
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

        [HttpDelete("pay")]
        public async Task<IActionResult> PayCart()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                UserResponse user = await database.User.Select(
                s => new UserResponse
                {
                    Id = s.Id,
                    Email = s.Email,
                    Pseudo = s.Pseudo,
                    Role = s.Role
                }
            ).FirstOrDefaultAsync(s => s.Id == userId);
            }
            else
            {
                return Unauthorized("Aucun utilisateur connecté !");
            }

            var cart = await database.Cart.Include(c => c.Items).FirstOrDefaultAsync(c => c.User_Id == userId);

            if (cart == null)
            {
                return NotFound();
            }

            decimal total = cart.Total_Price;
            database.Cart.Attach(cart);
            database.Cart.Remove(cart);
            await database.SaveChangesAsync();


            return Ok($"Payment successful, total cost : {total} euros");
        }
    }
}