using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/departamento")]
    public class DepartamentoController : BaseController<DepartamentoModel, DepartamentoEntity>
    {
        public DepartamentoController(IHttpContextAccessor accessor,
            IDepartamentoRepository departamentoRepository) : base(accessor, departamentoRepository) { }
    }
}
