using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp {
    internal class Program {
        static void Main(string[] args) {
            string url = "https://localhost:7252/Product";

            var client = new RestClient(url);

            var request = new RestRequest();

            var response = client.Get(request);

            Console.WriteLine(response.Content.ToString());

            Console.Read();
        }
    }
}
