using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iBay.Models
{
    public class Cart
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int User_Id { get; set; }

        [Required]
        public List<CartItem> Items { get; set; }

        [Required]
        public decimal Total_Price { get; set; }

        public Cart(int id, int user_Id, List<CartItem> items, decimal totalPrice) {
            Id = id;
            User_Id = user_Id;
            Items = items;
            Total_Price = totalPrice;
        }

        public Cart() {
            Items = new List<CartItem>();
        }
    }
}