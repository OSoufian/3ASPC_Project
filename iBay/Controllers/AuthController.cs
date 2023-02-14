using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iBay.MiddleWares;
using iBay.Models;
using iBay.Responses;

namespace iBay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MySQLConnection database;

        public AuthController(MySQLConnection database)
        {
            this.database = database;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post(Auth Auth)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await database.User.FirstOrDefaultAsync(x => (x.Pseudo == Auth.Pseudo | x.Email == Auth.Email));
            if (user != null)
            {
                return Conflict();
            }

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(Auth.Password, out passwordHash, out passwordSalt);

            User newUser = new User()
            {
                Email = Auth.Email,
                Pseudo = Auth.Pseudo,
                Password_Hash = passwordHash,
                Password_Salt = passwordSalt
            };

            database.Add(newUser);
            await database.SaveChangesAsync();

            UserResponse userResponse = new UserResponse()
            {
                Id = newUser.Id,
                Email = Auth.Email,
                Pseudo = Auth.Pseudo,
                Role = newUser.Role
            };

            return Created($"users/{newUser.Id}", userResponse);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (System.Security.Cryptography.HMACSHA512 hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await database.User.FirstOrDefaultAsync(x => x.Pseudo == login.Pseudo);
            if (user == null)
                return NotFound("Utilisateur inexistant !");

            if (!VerifyPassword(login.Password, user.Password_Hash, user.Password_Salt))
                return Unauthorized("Mot de passe incorrect");

            UserResponse userResponse = await database.User.Select(
                x => new UserResponse
                {
                    Id = x.Id,
                    Email = x.Email,
                    Pseudo = x.Pseudo,
                    Role = x.Role
                }
            ).FirstOrDefaultAsync(x => x.Pseudo == login.Pseudo);
            return Ok(userResponse);
        }

        private bool VerifyPassword(string password, byte[]? passwordHash, byte[]? passwordSalt)
        {
            if (passwordHash == null || passwordSalt == null) return false;
            using (System.Security.Cryptography.HMACSHA512 hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
    }
}