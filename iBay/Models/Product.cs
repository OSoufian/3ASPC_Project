using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace iBay.Models {
    public class Product {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [Url]
        public string? Image { get; set; }

        [Required]
        [Range(0, 10000000000000)]
        public int Price { get; set; }

        [Required]
        public int Available { get; set; }

        [Required]
        public DateTime Added_Time { get; set; }

        public Product(string name, int price, int available, DateTime AddedTime, string image) {
            Name = name;
            Price = price;
            Available = available;
            AddedTime = AddedTime;
            Image = image;
        }

        public Product() {
            Price = -1;
            Available = 0;
            Added_Time = new DateTime(2022, 2, 11);
        }
    }

}