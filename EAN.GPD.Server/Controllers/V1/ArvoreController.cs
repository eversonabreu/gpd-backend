using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/arvore")]
    public class ArvoreController : BaseController<ArvoreModel, ArvoreEntity>
    {
        public ArvoreController(IHttpContextAccessor accessor,
            IArvoreRepository arvoreRepository) : base(accessor, arvoreRepository) { }
    }
}
