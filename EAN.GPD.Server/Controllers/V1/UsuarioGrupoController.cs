using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/usuario-grupo")]
    public class UsuarioGrupoController : BaseController<UsuarioGrupoModel, UsuarioGrupoEntity>
    {
        public UsuarioGrupoController(IHttpContextAccessor accessor,
            IUsuarioGrupoRepository usuarioGrupoRepository) : base(accessor, usuarioGrupoRepository) { }
    }
}
