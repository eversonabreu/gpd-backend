using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/indicador")]
    public class IndicadorController : BaseController<IndicadorModel, IndicadorEntity>
    {
        public IndicadorController(IHttpContextAccessor accessor,
            IIndicadorRepository indicadorRepository) : base(accessor, indicadorRepository) { }
    }
}
