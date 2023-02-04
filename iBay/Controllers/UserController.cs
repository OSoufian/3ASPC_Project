using iBay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        public async Task<HttpStatusCode> Post(User user)
        {
            //return Enumerable.Range(1, 5).Select(index => new User
            //{
            //    Pseudo = "loki"
            //})
            //.ToArray();
            User newUser = new User() {
                Email = user.Email,
                Pseudo = user.Pseudo,
                Password = user.Password,
                Role = user.Role
            };

            database.Add(newUser);
            await database.SaveChangesAsync();

            return HttpStatusCode.Created;

        }
        [HttpGet(Name = "GetUser")]
        public IEnumerable<User> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new User {
                Pseudo = "loki"
            })
            .ToArray();
        }
        [HttpPut(Name = "UpdateUser")]
        public IEnumerable<User> Put()
        {
            return Enumerable.Range(1, 5).Select(index => new User
            {
                Pseudo = "loki"
            })
            .ToArray();
        }
        [HttpDelete(Name = "DeleteUser")]
        public IEnumerable<User> Delete()
        {
            return Enumerable.Range(1, 5).Select(index => new User
            {
                Pseudo = "loki"
            })
            .ToArray();
        }

    }
}