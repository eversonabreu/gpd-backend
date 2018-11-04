using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace EAN.GPD.Server.Controllers
{
    public class BaseController<TModel, TEntity> : ControllerBase where TModel : BaseModel where TEntity : BaseEntity
    {
    }
}
