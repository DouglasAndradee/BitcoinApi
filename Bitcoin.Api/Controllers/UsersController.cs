using Microsoft.AspNetCore.Mvc;
using Bitcoin.Api.Data;
using Bitcoin.Api.Models;

namespace Bitcoin.Api.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersDb _userDb;

        public UsersController(UsersDb userDb)
        {
            _userDb = userDb;
        }

        /// <summary>
        /// Cria um usuário e o grava no banco.
        /// </summary>
        /// <response code = "200">
        /// O password é enviado cryptografado para o banco de dados.
        /// </response>

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (user.Email == null || user.Password == null || user.Role == null)
            {
                return UnprocessableEntity();
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            user.Password = passwordHash;
            _userDb.Create(user);
            return Ok();
        }

    }
}