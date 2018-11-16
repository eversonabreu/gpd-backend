using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EAN.GPD.Server.Controllers
{
    [Route("estado-municipio")]
    public class EstadoMunicipioController : BaseController<BaseModel, BaseEntity>
    {
        public EstadoMunicipioController(IHttpContextAccessor accessor) : base(accessor, null) { }

        [HttpGet]
        [Route("obter-estados")]
        public IEnumerable<EstadoDto> ObterEstados()
        {
            var result = new List<EstadoDto>();
            var sql = DatabaseProvider.NewQuery("select * from estado order by nome");
            sql.ExecuteQuery();
            sql.ForEach(() =>
            {
                result.Add(new EstadoDto
                {
                    IdEstado = sql.GetLong("IDESTADO"),
                    Nome = sql.GetString("NOME"),
                    Sigla = sql.GetString("SIGLA")
                });
            });
            return result;
        }

        [HttpGet]
        [Route("obter-municipios/{idEstado:long}")]
        public IEnumerable<MunicipioDto> ObterMunicipios(long idEstado)
        {
            var result = new List<MunicipioDto>();
            var sql = DatabaseProvider.NewQuery($"select a.idmunicipio, a.nome, b.idestado, b.nome estadonome, b.sigla from minicipio a inner join estado b on (a.idestado = b.idestado) where b.idestado = {idEstado} order by a.nome");
            sql.ExecuteQuery();
            sql.ForEach(() =>
            {
                result.Add(new MunicipioDto
                {
                    IdMunicipio = sql.GetLong("IDMUNICIPIO"),
                    Nome = sql.GetString("NOME"),
                    Estado = new EstadoDto
                    {
                        IdEstado = sql.GetLong("IDESTADO"),
                        Nome = sql.GetString("ESTADONOME"),
                        Sigla = sql.GetString("SIGLA")
                    }
                });
            });
            return result;
        }

        [HttpGet]
        [Route("obter-estado/{idMunicipio:long}")]
        public EstadoDto ObterEstado(long idMunicipio)
        {
            var sql = DatabaseProvider.NewQuery($"select b.idestado, b.nome, b.sigla from minicipio a inner join estado b on (a.idestado = b.idestado) where a.idmunicipio = {idMunicipio}");
            sql.ExecuteQuery();
            return new EstadoDto
            {
                IdEstado = sql.GetLong("IDESTADO"),
                Nome = sql.GetString("NOME"),
                Sigla = sql.GetString("SIGLA")
            };
        }
    }

    public struct EstadoDto
    {
        public long IdEstado { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
    }

    public struct MunicipioDto
    {
        public long IdMunicipio { get; set; }
        public string Nome { get; set; }
        public EstadoDto Estado { get; set; }
    }
}
