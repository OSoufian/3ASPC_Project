using iBay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<User> Post(User user)
        {
            //return Enumerable.Range(1, 5).Select(index => new User
            //{
            //    Pseudo = "loki"
            //})
            //.ToArray();
            User newUser entity = new User() {
                Email = User.Email,
                LastName = User.LastName,
                Username = User.Username,
                Password = User.Password,
                EnrollmentDate = User.EnrollmentDate
            };
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