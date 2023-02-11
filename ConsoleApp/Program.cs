using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ConsoleApp {
    internal class Program {
        static void Main(string[] args) {
            mainMenu();
        }

        static void mainMenu() {
            Console.WriteLine("-- iBay --\n");
            Console.WriteLine("Products [P]");
            Console.WriteLine("Exit the application [Q]");
            Console.WriteLine("\n>");

            string input = Console.ReadLine();

            while (!isValidMainInput(input)) {
                Console.WriteLine("Veuillez entrer une commande valide :  ");
                input = Console.ReadLine();
            };
        }

        static void productMenu() {
            Console.WriteLine("-- Product --\n");
            Console.WriteLine("Get Products [G]");
            Console.WriteLine("Get Product by ID [I]");
            Console.WriteLine("Add Product [A]");
            Console.WriteLine("Edit Product [E]");
            Console.WriteLine("Delete Product [D]");
            Console.WriteLine("CANCEL [C]");
            Console.WriteLine("\n>");

            string input = Console.ReadLine();

            while (!isValidProductInput(input)) {
                Console.WriteLine("Veuillez entrer une commande valide :  ");
                input = Console.ReadLine();
            };


        }

        static bool isValidMainInput(string input) {
            bool valid = true;
            switch (input) {
                case "P":
                case "p":
                    productMenu();
                    break;
                case "q":
                case "Q":
                    break;
                default:
                    valid = false;
                    break;
            }
            return valid;
        }

        static bool isValidProductInput(string input) {
            bool valid = true;
            switch (input) {
                case "G":
                case "g":
                    getProducts();
                    break;
                case "I":
                case "i":
                    getProductById();
                    break;
                case "A":
                case "a":
                    addProduct();
                    break;
                case "c":
                case "C":
                    break;
                default:
                    valid = false;
                    break;
            }
            return valid;
        }

        static void getProducts() {
            Console.WriteLine("Connexion à l'API ...");
            var client = new RestClient("https://localhost:7252/Product");
            var request = new RestRequest();
            var response = client.Get(request);
            Console.WriteLine(response.Content.ToString());
            // TODO : Bien afficher le JSON
            Console.Read();
        }

        static void getProductById() {
            Console.WriteLine("Entrez id:");
            string idInput = Console.ReadLine();
            int id;
            while (!int.TryParse(idInput, out id)) {
                Console.WriteLine("Entrez un id valide (un nombre) :  ");
                idInput = Console.ReadLine();
            };

            Console.WriteLine("Connexion à l'API ...");
            var client = new RestClient("https://localhost:7252/Product/" + id);
            var request = new RestRequest();
            var response = client.Get(request);
            Console.WriteLine(response.Content.ToString());
            // TODO : Bien afficher le JSON
            Console.Read();
        }

        static void addProduct() {
            Console.WriteLine("Entrez le nom:");
            string name = Console.ReadLine();

            Console.WriteLine("Entrez le lien de l'image :");
            string image = Console.ReadLine();

            Console.WriteLine("Entrez le prix:");
            string priceInput = Console.ReadLine();
            int price;
            while (!int.TryParse(priceInput, out price)) {
                Console.WriteLine("Entrez un prix valide (un nombre) :  ");
                priceInput = Console.ReadLine();
            };

            Console.WriteLine("Entrez la validité :");
            string availableInput = Console.ReadLine();
            int available;
            while (!int.TryParse(availableInput, out available)) {
                Console.WriteLine("Entrez un prix valide (un nombre) :  ");
                availableInput = Console.ReadLine();
            };

            var client = new RestClient("https://localhost:7252/Product");
            var request = new RestRequest();
            Product product = new Product { Name = name, Image = image, Price = price, Available = available, Added_Time = DateTime.Now };
            request.AddJsonBody(product);
            var response = client.Post(request);
            Console.WriteLine(response.Content.ToString());
            // TODO : Bien afficher le JSON
            Console.Read();
        }
    }
}
