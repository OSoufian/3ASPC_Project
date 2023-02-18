using System.ComponentModel.DataAnnotations;

namespace iBay.Models {
    public class CartItem {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
