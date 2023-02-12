﻿using System.ComponentModel.DataAnnotations;

namespace iBay.Models {
    public class Auth {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(50)]
        public string? Pseudo { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string? Role { get; set; }

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