using System.ComponentModel.DataAnnotations;

namespace iBay
{
    public class Product
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [Url]
        public string? Image { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public bool Available { get; set; }

        [Required]
        public DateTime Added_time { get; set; }
    }
}