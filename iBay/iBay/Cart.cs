using System.ComponentModel.DataAnnotations;

namespace iBay {
    public class Cart {
        [Required]
        public List<Product>? Products { get; set; }
    }
}