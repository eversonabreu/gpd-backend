using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/unidade-medida")]
    public class UnidadeMedidaController : BaseController<UnidadeMedidaModel, UnidadeMedidaEntity>
    {
        public UnidadeMedidaController(IHttpContextAccessor accessor,
            IUnidadeMedidaRepository unidadeMedidaRepository) : base(accessor, unidadeMedidaRepository) { }
    }
}
