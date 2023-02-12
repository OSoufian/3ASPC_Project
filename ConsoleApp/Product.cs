
using System;

namespace ConsoleApp {
    public class Product {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }

        public int Price { get; set; }

        public int Available { get; set; }

        public DateTime Added_Time { get; set; }

        public Product(string name, int price, int available, DateTime AddedTime, string image) {
            Name = name;
            Price = price;
            Available = available;
            Added_Time = AddedTime;
            Image = image;
        }

        public Product() {
            Price = -1;
            Available = 0;
            Added_Time = new DateTime(2017, 8, 24);
        }
    }

}