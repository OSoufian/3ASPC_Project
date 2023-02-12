//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace iBay.Models
//{
//    public class Cart
//    {
//        [Required]
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public int UserId { get; set; }

//        [Required]
//        public List<Product> Products { get; set; }

//        public Cart()
//        {
//            Products = new List<Product>();
//        }

//        public Cart(List<Product> products)
//        {
//            Products = products;
//        }
//    }
//}