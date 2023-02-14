using System.ComponentModel.DataAnnotations;

namespace iBay.Models {
    public class User {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Pseudo { get; set; }

        [StringLength(200)]
        public byte[]? Password_Hash { get; set; }

        [StringLength(200)]
        public byte[]? Password_Salt { get; set; }

        [StringLength(10)]
        public string? Role { get; set; }

        public User(string email, string pseudo, byte[]? passwordHash, byte[]? passwordSalt, string? role = "user") {
            Email = email;
            Pseudo = pseudo;
            Password_Hash = passwordHash;
            Password_Salt = passwordSalt;
        }

        public User() {
            Email = "e";
            Pseudo = "ps";
            Role = "user";
        }
    }
}