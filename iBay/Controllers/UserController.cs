using iBay.Models;
using iBay.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iBay.Controllers {
    //[Authorize]
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase {
        private readonly MySQLConnection database;

        public UserController(MySQLConnection database) {
            this.database = database;
        }

        [HttpGet()]
        public async Task<IActionResult> Get() {
            List<UserResponse> List = await database.User.Select(
                s => new UserResponse {
                    Id = s.Id,
                    Email = s.Email,
                    Pseudo = s.Pseudo,
                    Role = s.Role
                }
            ).ToListAsync();

            if (List.Count < 0) {
                return NotFound("ouups");
            } else {
                return Ok(List);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int Id) {
            if (Id == 0) return BadRequest();
            UserResponse user = await database.User.Select(
                s => new UserResponse
                {
                    Id = s.Id,
                    Email = s.Email,
                    Pseudo = s.Pseudo,
                    Role = s.Role
                }
            ).FirstOrDefaultAsync(s => s.Id == Id);

            if (user == null) {
                return NotFound("Pas d'utilisateur");
            } else {
                return Ok(user);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Auth user, int Id) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            byte[] passwordHash, passwordSalt;
            User updatedUser = await database.User.FirstOrDefaultAsync(s => s.Id == Id);
            if (updatedUser == null) return BadRequest();
            Auth defaultUser = new Auth();

            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
            updatedUser.Email = user.Email == defaultUser.Email ? updatedUser.Email : user.Email;
            updatedUser.Pseudo = user.Pseudo == defaultUser.Pseudo ? updatedUser.Pseudo : user.Pseudo;
            updatedUser.Password_Hash = user.Password == defaultUser.Password ? updatedUser.Password_Hash : passwordHash;
            updatedUser.Password_Salt = user.Password == defaultUser.Password ? updatedUser.Password_Salt : passwordSalt;
            updatedUser.Role = user.Role == defaultUser.Role ? updatedUser.Role : user.Role;

            database.Update(updatedUser);
            await database.SaveChangesAsync();

            UserResponse userResponse = await database.User.Select(
                s => new UserResponse {
                    Id = s.Id,
                    Email = s.Email,
                    Pseudo = s.Pseudo,
                    Role = s.Role
                }
            ).FirstOrDefaultAsync(s => s.Id == Id);

            return Ok(userResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int Id) {
            User user = new User() {
                Id = Id
            };

            database.User.Attach(user);
            database.User.Remove(user);
            await database.SaveChangesAsync();
            return Ok("L'utilisateur " + user.Id + " a bien été supprimé !");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (System.Security.Cryptography.HMACSHA512 hmac = new System.Security.Cryptography.HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}