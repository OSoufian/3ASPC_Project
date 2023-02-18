using System.ComponentModel.DataAnnotations;

namespace iBay.Models {
    public class CartItem {
        [Required]
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        [Required]
        public int Product_Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
