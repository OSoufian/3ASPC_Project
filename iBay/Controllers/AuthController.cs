using iBay.MiddleWares;
using iBay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iBay.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase {
        private readonly MySQLConnection database;

        public AuthController(MySQLConnection database) {
            this.database = database;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post(Auth Auth) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(Auth.Password, out passwordHash, out passwordSalt);

            User newUser = new User() {
                Email = Auth.Email,
                Pseudo = Auth.Pseudo,
                Password_Hash = passwordHash,
                Password_Salt = passwordSalt,
                Role = Auth.Role
            };

            database.Add(newUser);
            await database.SaveChangesAsync();

            return Created($"User/{newUser.Id}", newUser);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(Login login) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await database.User.FirstOrDefaultAsync(x => x.Pseudo == login.Pseudo);
            if (user == null)
                return NotFound();

            if (!VerifyPassword(login.Password, user.Password_Hash, user.Password_Salt))
                return Unauthorized();

            return Ok(user);
        }

        private bool VerifyPassword(string password, byte[]? passwordHash, byte[]? passwordSalt) {
            if (passwordHash == null || passwordSalt == null) return false;
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
    }
}