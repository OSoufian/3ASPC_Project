using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using RestSharp;

using iBay.Models;

namespace ConsoleApp {
    internal class Program {
        static void Main(string[] args) {
            connectWebAPI();
        }

        public static void connectWebAPI() {
            HttpClient client = new HttpClient();
            Console.WriteLine("Connexion à l'API ...");
            client.BaseAddress = new Uri("https://localhost:7252/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("Product");
            if (response.IsSuccessStatusCode) {
                dynamic result = await response.Content.ReadAsStringAsync();
                Rootobject rootObject = JsonConvert.DeserializeObject<Rootobject>(result);

                foreach (var item in rootObject.Product) {
                    Console.WriteLine("{0}\t${1}\t{2}", item.id);
                }

                Console.ReadKey();
            }
        }

        
    }

    public class Rootobject {
        public List<Product> Product { get; set;}
    }
}
