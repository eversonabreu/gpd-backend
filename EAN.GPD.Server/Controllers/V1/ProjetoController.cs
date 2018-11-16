using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/projeto")]
    public class ProjetoController : BaseController<ProjetoModel, ProjetoEntity>
    {
        public ProjetoController(IHttpContextAccessor accessor,
            IProjetoRepository projetoRepository) : base(accessor, projetoRepository) { }
    }
}
