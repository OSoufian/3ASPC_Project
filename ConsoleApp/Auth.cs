using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp {
    internal class Auth {
            public int Id { get; set; }

            public string Email { get; set; }

            public string Pseudo { get; set; }

            public string Password { get; set; }

            public string Role { get; set; }

            public Auth(string email, string pseudo, string password, string role = "user") {
                Email = email;
                Pseudo = pseudo;
                Password = password;
                Role = role;
            }

            public Auth() {
                Email = "e";
                Pseudo = "ps";
                Password = "pa";
                Role = "u";
            }
        }
}
