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
        public int UserId { get; set; }

        [Required]
        public List<CartItem> Items { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public Cart(int id, int userId, List<CartItem> items, decimal totalPrice) {
            Id = id;
            UserId = userId;
            Items = items;
            TotalPrice = totalPrice;
        }

        public Cart() {
            Id = 0;
            UserId = 0;
            Items = new List<CartItem>();
            TotalPrice =0;
        }
    }
}