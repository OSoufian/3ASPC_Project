using System.ComponentModel.DataAnnotations;

namespace iBay.Responses {
    public class UserResponse {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Pseudo { get; set; }

        [StringLength(10)]
        public string? Role { get; set; }

        public UserResponse(string email, string pseudo, string? role = "user") {
            Email = email;
            Pseudo = pseudo;
        }

        public UserResponse() {
            Email = "e";
            Pseudo = "ps";
            Role = "user";
        }
    }
}