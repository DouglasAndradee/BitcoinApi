using Microsoft.AspNetCore.Mvc;
using Bitcoin.Api.Data;
using Bitcoin.Api.Models;
using System.Collections.Generic;

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
            return Ok(new { message = "Usuário cadastrado com sucesso." });
        }

        /// <summary>
        /// Obtem todos os usuários.
        /// </summary>

        [HttpGet]
        public IActionResult Get()
        {
            var users = new List<User>();
            var result = _userDb.Get();

            foreach (var user in result)
            {
                user.Password = "";
                users.Add(user);
            }
            return Ok(users);
        }

        /// <summary>
        /// Atualiza o usuário através da ID.
        /// </summary>

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User user)
        {
            var result = _userDb.GetById(id);

            if (result == null)
            {
                return NotFound("Usuário não encontrado");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            user.Password = passwordHash;
            _userDb.Update(id, user);
            return Ok(new { message = "Dados de usuário atualizados com sucesso" });
        }

        /// <summary>
        /// Deleta um usuário através da ID.
        /// </summary>

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteById(string id)
        {
            var result = _userDb.GetById(id);

            if (result == null)
            {
                return NotFound("Usuário não encontrado");
            }

            _userDb.DeleteById(result.Id);

            return Ok(new { message = "Usuário removido co sucesso." });
        }

    }
}