using EAN.GPD.Domain.Repositories;
using EAN.GPD.Domain.Utils;
using EAN.GPD.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace EAN.GPD.Server.Controllers
{
    [Route("authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly string login;
        private readonly string password;
        private readonly IUsuarioRepository repository;

        public AuthenticationController(IHttpContextAccessor accessor,
                                        IUsuarioRepository repository)
        {
            string value = accessor.HttpContext.Request.Headers["Authorization"];
            string newValue = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            string login = string.Empty;
            int indexStart = 0;

            for (int index = 0; index < newValue.Length; index++)
            {
                indexStart = index;
                char ch = newValue[index];
                if (ch != ':')
                {
                    login += ch;
                }
                else
                {
                    indexStart++;
                    break;
                }
            }

            string password = newValue.Substring(indexStart);
            this.login = login;
            this.password = password;
            this.repository = repository;
        }

        [HttpGet]
        public string GetToken()
        {
            var usuario = repository.Find($"Login = '{login}'");
            if (usuario != null && usuario.Ativo && usuario.SenhaLogin == password)
            {
                return GeradorToken.GetToken(usuario.IdUsuario);
            }

            throw new Exception("Login ou senha inválidos.");
        }
    }
}
