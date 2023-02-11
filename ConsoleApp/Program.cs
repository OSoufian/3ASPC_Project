using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iBay.models;
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
            Console.WriteLine("-- Produit --\n");
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
            var client = new RestClient("https://localhost:7252/Product");
            var request = new RestRequest();
            var response = client.Get(request);
            Console.WriteLine(response.Content.ToString());
            Console.Read();
        }
    }
}
