using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAN.GPD.Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> Filter(string whereExpression = null);
        (IEnumerable<TEntity>, int) Filter(uint page = 0, uint count = 10, string whereExpression = null);
        TEntity Find(string whereExpression);
        TEntity GetOne(long id);
        Task GenerateAuditAsync(TypeAudit typeAudit, string nameTable, long idTabela, UserLogged userLogged, string entity = null);
    }
}
