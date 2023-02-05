using iBay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace iBay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MySQLConnection database;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, MySQLConnection database)
        {
            this.database = database;
            _logger = logger;
        }

        [HttpPost(Name = "AddUser")]
        public async Task<IActionResult> Post(User user)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

            User newUser = new User() {
                Email = user.Email,
                Pseudo = user.Pseudo,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = user.Role
            };

            database.Add(newUser);
            await database.SaveChangesAsync();

            return Created($"User/{newUser.Id}", newUser);
        }

        [HttpGet()]
        public async Task<ActionResult<List<User>>> Get()
        {
            var List = await database.User.Select(
                s => new User
                {
                    Id = s.Id,
                    Email = s.Email,
                    Pseudo = s.Pseudo,
                    PasswordHash = s.PasswordHash,
                    PasswordSalt = s.PasswordSalt,
                    Role = s.Role
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound("ouups");
            }
            else
            {
                return List;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int Id) {
            User user = await database.User.Select(
                    s => new User
                    {
                        Id = s.Id,
                        Email = s.Email ?? "nope",
                        Pseudo = s.Pseudo ?? "nope",
                        PasswordHash = s.PasswordHash ?? "nope",
                        PasswordSalt = s.PasswordSalt ?? "nope",
                        Role = s.Role ?? "nope"
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (user == null)
            {
                return NotFound("Pas d'utilisateur");
            }
            else
            {
                return user;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(User user, int Id)
        {
            byte[] passwordHash, passwordSalt;
            User updatedUser = await database.User.FirstOrDefaultAsync(s => s.Id == Id);

            password? CreatePasswordHash(user.Password, out passwordHash, out passwordSalt) : null;

            updatedUser.Email = user.Email == "e" ? updatedUser.Email : user.Email;
            updatedUser.Pseudo = user.Pseudo == "ps" ? updatedUser.Pseudo : user.Pseudo;
            updatedUser.PasswordHash = user.PasswordHash == "pah" ? updatedUser.PasswordHash : user.PasswordHash
            updatedUser.PasswordSalt = user.PasswordSalt == "pas" ? updatedUser.PasswordSalt : user.PasswordSalt
            updatedUser.Role = user.Role == "u" ? updatedUser.Role : user.Role;


            await database.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            User user = new User()
            {
                Id = Id
            };

            database.User.Attach(user);
            database.User.Remove(user);
            await database.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("login")]
        public async Task<User> Login(string pseudo, string password)
        {
            var user = await database.User.FirstOrDefaultAsync(x => x.Pseudo == pseudo);
            if (user == null)
                return NotFound();

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized();

            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string pseudo) {
            if (await database.User.AnyAsync(x => x.Pseudo == pseudo))
                return true;
            return false;
        }
    }
}