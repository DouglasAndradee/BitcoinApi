using Microsoft.AspNetCore.Mvc;
using Bitcoin.Api.Models;
using Bitcoin.Api.Repositories;
using Bitcoin.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Bitcoin.Api.Data;

namespace webapi.mongodb.Controllers
{
    [Route("v1")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly UsersDb _userDb;

        public HomeController(UsersDb userDb)
        {
            _userDb = userDb;
        }

        /// <summary>
        /// Faz a autenticação do usuário.
        /// </summary>
        /// 
        /// <response code = "200">
        /// Usando somente os campos abaixo:<br />
        /// email: "string",<br />
        /// passwor: "sring"<br />
        /// Retorna um token<br />
        /// </response>

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]

        public IActionResult Authentication(Authentication body)
        {
            if (body.Email == null || body.Password == null)
            {
                return UnprocessableEntity();
            }

            var result = _userDb.GetByEmail(body.Email);

            if (result == null)
            {
                return NotFound();
            }

            bool verified = BCrypt.Net.BCrypt.Verify(body.Password, result.Password);
            if (verified == false)
            {
                return Unauthorized(new { message = "Email/Password incorretos" });
            }

            var user = UserRepository.Get(result.Username, result.Email, body.Password, result.Role);
            var token = TokenService.GenerateToken(user);

            return Ok(new { message = token });
        }

    }

}
