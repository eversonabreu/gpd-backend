using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using EAN.GPD.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/usuario")]
    public class UsuarioController : BaseController<UsuarioModel, UsuarioEntity>
    {
        public UsuarioController(IHttpContextAccessor accessor,
            IUsuarioRepository usuarioRepository) : base(accessor, usuarioRepository) { }

        private void CodificarSenhaUsuario(UsuarioModel model)
        {
            string senha = Encoding.UTF8.GetString(Convert.FromBase64String(model.SenhaLogin));
            model.SenhaLogin = Criptografia.Codificar(senha);
        }

        protected override void BeforePost(UsuarioModel model)
        {
            CodificarSenhaUsuario(model);
        }

        protected override void BeforePut(UsuarioModel model)
        {
            CodificarSenhaUsuario(model);
        }
    }
}
