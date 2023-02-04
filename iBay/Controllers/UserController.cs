using iBay.Models;
using Microsoft.AspNetCore.Mvc;

namespace iBay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "AddUser")]
        public IEnumerable<User> Post()
        {
            return Enumerable.Range(1, 5).Select(index => new User
            {
                Pseudo = "loki"
            })
            .ToArray();
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