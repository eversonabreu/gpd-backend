using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers.V1
{
    [Route("v1/cargo")]
    public class CargoController : BaseController<CargoModel, CargoEntity>
    {
        public CargoController(IHttpContextAccessor accessor,
            ICargoRepository cargoRepository) : base(accessor, cargoRepository) { }
    }
}
