using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/movimento")]
    public class MovimentoController : BaseController<MovimentoModel, MovimentoEntity>
    {
        public MovimentoController(IHttpContextAccessor accessor,
            IMovimentoRepository movimentoRepository) : base(accessor, movimentoRepository) { }
    }
}
