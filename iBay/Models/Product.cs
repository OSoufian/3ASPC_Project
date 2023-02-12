using System.ComponentModel.DataAnnotations;

namespace iBay.Models {
    public class Product {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Url]
        public string? Image { get; set; }

        [Required]
        [Range(0, 100000000)]
        public int Price { get; set; }

        [Required]
        public int Available { get; set; }

        public DateTime Added_Time { get; set; }

        public Product(string name, int price, int available, DateTime addedTime, string image) {
            Name = name;
            Price = price;
            Available = available;
            Added_Time = addedTime;
            Image = image;
        }

        public Product() {
            Name = "Inconnu";
            Price = -1;
            Image = "";
            Available = 0;
            Added_Time = new DateTime(1970, 1, 1);
        }
    }

}