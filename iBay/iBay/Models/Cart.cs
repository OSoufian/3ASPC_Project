using System.ComponentModel.DataAnnotations;

namespace iBay.Models
{
    public class Cart
    {
        [Required]
        public List<Product>? Products { get; set; }
    }
}