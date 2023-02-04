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
            User newUser = new User() {
                Email = user.Email,
                Pseudo = user.Pseudo,
                Password = user.Password,
                Role = user.Role
            };

            database.Add(newUser);
            await database.SaveChangesAsync();

            //newUser.Remove("password");

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
                    Password = s.Password,
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
                        Password = s.Password ?? "nope",
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
            User updatedUser = await database.User.FirstOrDefaultAsync(s => s.Id == Id);

            updatedUser.Email = user.Email == "e" ? updatedUser.Email : user.Email;
            updatedUser.Pseudo = user.Pseudo == "ps" ? updatedUser.Pseudo : user.Pseudo;
            updatedUser.Password = user.Password == "pa" ? updatedUser.Password : user.Password;
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
    }
}