using iBay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace iBay.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase {
        private readonly MySQLConnection database;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, MySQLConnection database) {
            this.database = database;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post(Auth Auth) {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(Auth.Password, out passwordHash, out passwordSalt);

            User newUser = new User() {
                Email = Auth.Email,
                Pseudo = Auth.Pseudo,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = Auth.Role
            };

            database.Add(newUser);
            await database.SaveChangesAsync();

            return Created($"User/{newUser.Id}", newUser);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string pseudo, string password) {
            var user = await database.User.FirstOrDefaultAsync(x => x.Pseudo == pseudo);
            if (user == null)
                return NotFound();

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized();

            return Ok(user);
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> AuthExists(string pseudo) {
            if (await database.User.AnyAsync(x => x.Pseudo == pseudo))
                return true;
            return false;
        }
    }
}