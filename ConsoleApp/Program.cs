using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

//using iBay.Models;

namespace ConsoleApp {
    internal class Program {
        static void Main(string[] args) {
            mainMenu();
        }

        static void mainMenu() {
            Console.Clear();
            Console.WriteLine("-- iBay --\n");
            Console.WriteLine("Products [P]");
            Console.WriteLine("Login [L]");
            Console.WriteLine("Register [R]");
            Console.WriteLine("Manage Users (only Admin) [U]");
            Console.WriteLine("Exit the application [Q]");
            Console.WriteLine("\n>");

            string input = Console.ReadLine();

            while (!isValidMainInput(input)) {
                Console.WriteLine("Veuillez entrer une commande valide :  ");
                input = Console.ReadLine();
            };
        }

        static void productsMenu() {
            Console.Clear();
            Console.WriteLine("-- Products --\n");
            Console.WriteLine("Get Products [G]");
            Console.WriteLine("Get Product by ID [I]");
            Console.WriteLine("Add Product [A]");
            Console.WriteLine("Edit Product [E]");
            Console.WriteLine("Delete Product [D]");
            Console.WriteLine("Cancel [C]");
            Console.WriteLine("\n>");

            string input = Console.ReadLine();

            while (!isValidProductsInput(input)) {
                Console.WriteLine("Veuillez entrer une commande valide :  ");
                input = Console.ReadLine();
            };
        }

        static void usersMenu() {
            Console.Clear();
            Console.WriteLine("-- Users --\n");
            Console.WriteLine("Get Users [G]");
            Console.WriteLine("Get User by ID [I]");
            Console.WriteLine("Add User [A]");
            Console.WriteLine("Edit User [E]");
            Console.WriteLine("Delete User [D]");
            Console.WriteLine("Cancel [C]");
            Console.WriteLine("\n>");

            string input = Console.ReadLine();

            while (!isValidUsersInput(input)) {
                Console.WriteLine("Veuillez entrer une commande valide :  ");
                input = Console.ReadLine();
            };
        }

        static bool isValidMainInput(string input) {
            bool valid = true;
            switch (input) {
                case "P":
                case "p":
                    productsMenu();
                    break;
                case "L":
                case "l":
                    login();
                    break;
                case "R":
                case "r":
                    register();
                    break;
                case "U":
                case "u":
                    usersMenu();
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

        static bool isValidProductsInput(string input) {
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
                case "E":
                case "e":
                    editProduct();
                    break;
                case "D":
                case "d":
                    deleteProduct();
                    break;
                case "c":
                case "C":
                    mainMenu();
                    break;
                default:
                    valid = false;
                    break;
            }
            return valid;
        }

        static bool isValidUsersInput(string input) {
            bool valid = true;
            switch (input) {
                case "G":
                case "g":
                    getUsers();
                    break;
                case "I":
                case "i":
                    getUserById();
                    break;
                case "A":
                case "a":
                    addProduct();
                    break;
                case "E":
                case "e":
                    editProduct();
                    break;
                case "D":
                case "d":
                    deleteUser();
                    break;
                case "c":
                case "C":
                    mainMenu();
                    break;
                default:
                    valid = false;
                    break;
            }
            return valid;
        }

        static void getProducts() {
            Console.Clear();
            Console.WriteLine("Connexion à l'API ...");
            var client = new RestClient("https://localhost:7252/products");
            var request = new RestRequest();
            var response = client.Get(request);
            Console.WriteLine(formatJson(response.Content.ToString()));
            Console.Read();
            productsMenu();
        }

        static void getProductById() {
            Console.Clear();
            Console.WriteLine("Entrez id:");
            string idInput = Console.ReadLine();
            int id;
            while (!int.TryParse(idInput, out id)) {
                Console.WriteLine("Entrez un id valide (un nombre) :  ");
                idInput = Console.ReadLine();
            };

            Console.WriteLine("Connexion à l'API ...");
            var client = new RestClient("https://localhost:7252/products/" + id);
            var request = new RestRequest();
            var response = client.Get(request);
            Console.WriteLine(formatJson(response.Content.ToString()));
            Console.Read();
            productsMenu();
        }

        static void addProduct() {
            Console.Clear();
            Console.WriteLine("Entrez le nom:");
            string name = Console.ReadLine();

            Console.WriteLine("Entrez le lien de l'image :");
            string image = Console.ReadLine();
            Uri uriResult;
            bool result = Uri.TryCreate(image, UriKind.Absolute, out uriResult)
                && uriResult.Scheme == Uri.UriSchemeHttp;
            while (!result) {
                Console.WriteLine("Entrez un lien valide : ");
                image = Console.ReadLine();
                result = Uri.TryCreate(image, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            };

            Console.WriteLine("Entrez le prix:");
            string priceInput = Console.ReadLine();
            int price;
            while (!int.TryParse(priceInput, out price)) {
                Console.WriteLine("Entrez un prix valide (un nombre) : ");
                priceInput = Console.ReadLine();
            };

            Console.WriteLine("Entrez la validité [O/N] :");
            string availableInput = Console.ReadLine();
            while (availableInput != "O" && availableInput != "N") {
                Console.WriteLine("Entrez une commande valide (O ou N) : ");
                availableInput = Console.ReadLine();
            };

            int available = availableInput == "O" ? 1 : 0;

            var client = new RestClient("https://localhost:7252/products");
            var request = new RestRequest();
            Product product = new Product { Name = name, Image = image, Price = price, Available = available, Added_Time = DateTime.UtcNow };
            request.AddJsonBody(product);
            var response = client.Post(request);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(formatJson(response.Content.ToString()));
            Console.Read();
            productsMenu();
        }

        static void editProduct() {
            Console.Clear();
            Console.WriteLine("Entrez id:");
            string idInput = Console.ReadLine();
            int id;
            while (!int.TryParse(idInput, out id)) {
                Console.WriteLine("Entrez un id valide (un nombre) :  ");
                idInput = Console.ReadLine();
            };

            Console.WriteLine("Pour chaque propriété, laissez vide pour ne pas modifier:");
            Console.WriteLine("Entrez le nouveau nom:");
            string name = Console.ReadLine();

            Console.WriteLine("Entrez le nouveau lien de l'image :");
            string image = Console.ReadLine();

            Console.WriteLine("Entrez le nouveau prix:");
            string priceInput = Console.ReadLine();
            int price;
            while (!int.TryParse(priceInput, out price) && priceInput != "") {
                Console.WriteLine("Entrez un prix valide (un nombre) : ");
                priceInput = Console.ReadLine();
            };

            Console.WriteLine("Entrez la nouvelle validité [O/N] :");
            string availableInput = Console.ReadLine();
            while (availableInput != "O" && availableInput != "N" && availableInput != "") {
                Console.WriteLine("Entrez une commande valide (O ou N) : ");
                availableInput = Console.ReadLine();
            };

            int available = availableInput == "O" ? 1 : 0;



            var client = new RestClient("https://localhost:7252/products/" + id);
            var request = new RestRequest();
            Product product = new Product();
            if (name != "") product.Name = name;
            if (image != "") product.Image = image;
            if (priceInput != "") product.Price = price;
            if (availableInput != "") product.Available = available;

            request.AddJsonBody(product);
            var response = client.Put(request);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(formatJson(response.Content.ToString()));
            Console.Read();
            productsMenu();
        }

        static void deleteProduct() {
            Console.Clear();
            Console.WriteLine("Entrez id:");
            string idInput = Console.ReadLine();
            int id;
            while (!int.TryParse(idInput, out id)) {
                Console.WriteLine("Entrez un id valide (un nombre) :  ");
                idInput = Console.ReadLine();
            };

            Console.WriteLine("Connexion à l'API ...");
            var client = new RestClient("https://localhost:7252/products/" + id);
            var request = new RestRequest();
            var response = client.Delete(request);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);
            Console.Read();
            productsMenu();
        }

        static void login() {
            Console.Clear();
            Console.WriteLine("Entrez le nom d'utilisateur:");
            string pseudo = Console.ReadLine();

            Console.WriteLine("Entrez le mot de passe:");
            string password = Console.ReadLine();


            var client = new RestClient("https://localhost:7252/auth/login");
            var request = new RestRequest();
            Login login = new Login { Pseudo = pseudo, Password = password };
            request.AddJsonBody(login);
            RestResponse response = null;
            try {
                response = client.Post(request);

                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Content.ToString());

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            
            Console.Read();
            mainMenu();
        }

        static void register() {
            Console.Clear();
            Console.WriteLine("Entrez votre nom d'utilisateur:");
            string pseudo = Console.ReadLine();

            Console.WriteLine("Entrez votre mail:");
            string email = Console.ReadLine();
            while (!isValidEmail(email)) {
                Console.WriteLine("Entrez un email valide :  ");
                email = Console.ReadLine();
            };            

            Console.WriteLine("Entrez votre mot de passe:");
            string password = Console.ReadLine();


            var client = new RestClient("https://localhost:7252/auth/register");
            var request = new RestRequest();
            Auth register = new Auth { Pseudo = pseudo, Email = email, Password = password };
            request.AddJsonBody(register);
            RestResponse response = null;
            try {
                response = client.Post(request);

                Console.WriteLine(response.StatusCode);
                Console.WriteLine(formatJson(response.Content.ToString()));

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
            mainMenu();
        }

        static bool isValidEmail(string email) {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith(".")) {
                return false;
            }
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            } catch {
                return false;
            }
        }

        static void getUsers() {
            Console.Clear();
            Console.WriteLine("Connexion à l'API ...");
            var client = new RestClient("https://localhost:7252/users");
            var request = new RestRequest();
            var response = client.Get(request);
            Console.WriteLine(formatJson(response.Content.ToString()));
            Console.Read();
            usersMenu();
        }

        static void getUserById() {
            Console.Clear();
            Console.WriteLine("Entrez id:");
            string idInput = Console.ReadLine();
            int id;
            while (!int.TryParse(idInput, out id)) {
                Console.WriteLine("Entrez un id valide (un nombre) :  ");
                idInput = Console.ReadLine();
            };

            Console.WriteLine("Connexion à l'API ...");
            var client = new RestClient("https://localhost:7252/users/" + id);
            var request = new RestRequest();
            var response = client.Get(request);
            Console.WriteLine(formatJson(response.Content.ToString()));
            Console.Read();
            usersMenu();
        }

        static void deleteUser() {
            Console.Clear();
            Console.WriteLine("Entrez id:");
            string idInput = Console.ReadLine();
            int id;
            while (!int.TryParse(idInput, out id)) {
                Console.WriteLine("Entrez un id valide (un nombre) :  ");
                idInput = Console.ReadLine();
            };

            Console.WriteLine("Connexion à l'API ...");
            var client = new RestClient("https://localhost:7252/users/" + id);
            var request = new RestRequest();
            var response = client.Delete(request);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);
            Console.Read();
            usersMenu();
        }

        static string formatJson(string json) {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}
