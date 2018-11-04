using EAN.GPD.Domain.Entities;
using System.Collections.Generic;

namespace EAN.GPD.Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> Filter(string whereExpression = null);
        IEnumerable<TEntity> Filter(uint page = 0, uint count = 10, string whereExpression = null);
        TEntity Find(string whereExpression);
        TEntity GetOne(long id);
    }
}
