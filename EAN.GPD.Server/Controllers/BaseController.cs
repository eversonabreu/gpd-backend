using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Models;
using EAN.GPD.Domain.Repositories;
using EAN.GPD.Domain.Utils;
using EAN.GPD.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EAN.GPD.Server.Controllers
{
    public class BaseController<TModel, TEntity> : ControllerBase where TModel : BaseModel where TEntity : BaseEntity
    {
        private readonly UserLogged userLogged;
        private readonly HttpContext httpContext;
        private readonly IBaseRepository<TEntity> repository;

        public BaseController(IHttpContextAccessor accessor,
            IBaseRepository<TEntity> repository)
        {
            this.repository = repository;
            httpContext = accessor.HttpContext;
            string value = httpContext.Request.Headers["Authorization"];
            if (!GeradorToken.ValidateToken(value, out userLogged))
            {
                throw new Exception("Token inválido ou expirado.");
            }
        }

        protected UserLogged UserLogged { get => userLogged; }

        protected virtual void BeforePost(TModel model) { }

        protected virtual void BeforePut(TModel model) { }

        protected virtual void BeforeDelete(long id) { }

        [HttpPost]
        public virtual long Post([FromBody] TModel model)
        {
            if (!model.IsValid(out string messages))
            {
                throw new Exception(messages);
            }

            BeforePost(model);
            TEntity entity = model.ToEntity<TEntity>();
            long result = entity.Save();
            Task.Run(() => repository.GenerateAuditAsync(TypeAudit.Insert, entity.GetNameTable(), result, userLogged));
            return result;
        }

        [HttpPut]
        public virtual void Put([FromBody] TModel model)
        {
            if (!model.IsValid(out string messages))
            {
                throw new Exception(messages);
            }
            else if (model.GetId() is null)
            {
                throw new Exception("O 'ID' não foi definido para a solicitação de atualização de dados.");
            }

            BeforePut(model);
            TEntity oldEntity = repository.GetOne(model.GetId().Value);
            string jsonEntity = JsonConvert.SerializeObject(oldEntity);
            TEntity entity = model.ToEntity<TEntity>();
            entity.Save(model.GetId().Value);
            Task.Run(() => repository.GenerateAuditAsync(TypeAudit.Update, entity.GetNameTable(), model.GetId().Value, userLogged, jsonEntity));
        }

        [HttpDelete]
        [Route("{id:long}")]
        public virtual void Delete(long id)
        {
            BeforeDelete(id);
            TEntity oldEntity = repository.GetOne(id);
            string jsonEntity = JsonConvert.SerializeObject(oldEntity);
            BaseEntity.Delete<TEntity>(id);
            Task.Run(() => repository.GenerateAuditAsync(TypeAudit.Delete, oldEntity.GetNameTable(), id, userLogged, jsonEntity));
        }

        [HttpGet]
        [Route("{id:long}")]
        public virtual TEntity GetOne(long id) => repository.GetOne(id);

        [HttpGet]
        [Route("todos")]
        [Obsolete("Método não deve ser chamado em entidades que possuem muitos registros.")]
        public virtual IEnumerable<TEntity> GetAll() => repository.Filter(null);

        [HttpGet]
        [Route("todos-ativos")]
        [Obsolete("Método não deve ser chamado em entidades que possuem muitos registros.")]
        public virtual IEnumerable<TEntity> GetAllActives() => repository.Filter("Ativo = 'S'");

        [HttpGet]
        [Route("todos-com-paginacao")]
        public virtual IEnumerable<TEntity> GetAllPaged(uint pagina = 0, uint quantidade = 10) => repository.Filter(pagina, quantidade).Item1;

        [HttpGet]
        [Route("todos-ativos-com-paginacao")]
        public virtual IEnumerable<TEntity> GetAllActivesPaged(uint pagina = 0, uint quantidade = 10) => repository.Filter(pagina, quantidade, "Ativo = 'S'").Item1;

        [HttpGet]
        public virtual ResultSet<TEntity> Get([FromQuery] TEntity example, uint pagina = 0, uint quantidade = 10, string preFilter = null)
        {
            if (example is null)
            {
                return new ResultSet<TEntity>(null, 0);
            }

            bool IsPropertySetter(string queryUrl, string propertyName)
            {
                if (string.IsNullOrWhiteSpace(queryUrl))
                {
                    return false;
                }

                string queryUrlUpper = queryUrl.ToUpper();
                string propertyNameUpper = propertyName.ToUpper();
                return queryUrlUpper.Contains($"?{propertyNameUpper}=") || queryUrlUpper.Contains($"&{propertyNameUpper}=");
            }

            string queryString = httpContext.Request.QueryString.Value;
            var properties = example.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var stringsExpressions = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                var valueProperty = property.GetValue(example);
                if (valueProperty != null && IsPropertySetter(queryString, property.Name))
                {
                    var column = property.GetCustomAttribute<Column>();
                    if (column != null)
                    {
                        string name = (column.Name ?? property.Name).Trim().ToUpper();
                        string value = valueProperty.ToString().Trim().ToUpper().Replace("'", "#@$&*");
                        stringsExpressions.Add($"(replace(upper(trim(cast({name} as varchar))), '''', '#@$&*') like '%{value}%')");
                    }
                }
            }

            if (!stringsExpressions.Any())
            {
                var resultAll = repository.Filter(pagina, quantidade, preFilter);
                return new ResultSet<TEntity>(resultAll.Item1, resultAll.Item2);
            }

            string expressions = string.Join(" and ", stringsExpressions);
            if (!string.IsNullOrWhiteSpace(preFilter))
            {
                expressions += $" and ({preFilter})";
            }

            var result = repository.Filter(pagina, quantidade, expressions);
            return new ResultSet<TEntity>(result.Item1, result.Item2);
        }

        [HttpGet]
        [Route("consulta")]
        public virtual ResultSet<TEntity> GetSearch(string conteudo, uint pagina = 0, uint quantidade = 10, string preFilter = null)
        {
            if (string.IsNullOrWhiteSpace(conteudo))
		    {
                var resultAll = repository.Filter(pagina, quantidade, preFilter);
                return new ResultSet<TEntity>(resultAll.Item1, resultAll.Item2);
		    }

            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var stringsExpressions = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                var column = property.GetCustomAttribute<Column>();
                if (column != null)
                {
                    string name = (column.Name ?? property.Name).Trim().ToUpper();
                    string value = conteudo.ToString().Trim().ToUpper().Replace("'", "#@$&*");
                    stringsExpressions.Add($"(replace(upper(trim(cast({name} as varchar))), '''', '#@$&*') like '%{value}%')");
                }
            }

            if (!stringsExpressions.Any())
            {
                var resultAll = repository.Filter(pagina, quantidade, preFilter);
                return new ResultSet<TEntity>(resultAll.Item1, resultAll.Item2);
            }

            string expressions = string.Join(" or ", stringsExpressions);
            if (!string.IsNullOrWhiteSpace(preFilter))
            {
                expressions += $" and ({preFilter})";
            }

            var result = repository.Filter(pagina, quantidade, expressions);
            return new ResultSet<TEntity>(result.Item1, result.Item2);
        }
    }
}
