using System.ComponentModel.DataAnnotations;

namespace iBay.Models
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
        [Range(0, 10000000000000)]
        public int Price { get; set; }

        [Required]
        public bool Available { get; set; }

        [Required]
        public DateTime Added_time { get; set; }

        public Product(int price, bool available, DateTime AddedTime, string? image)
        {
            Price = price;
            Available = available;
            AddedTime = AddedTime;
            Image = image;
        }

        public Product() {
            Price = -1;
            Available = false;
            Added_time = new DateTime(2017, 8, 24);
        }
    }

}