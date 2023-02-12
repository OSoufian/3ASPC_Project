
using System;

namespace ConsoleApp {
    public class Product {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public int Price { get; set; }

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
            Price = 100000000;
            Available = 2;
            Added_Time = new DateTime(1970, 1, 1);
        }
    }

}