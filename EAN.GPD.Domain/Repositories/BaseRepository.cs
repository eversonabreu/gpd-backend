using EAN.GPD.Domain.Entities;
using EAN.GPD.Infrastructure.Database;
using System;
using System.Collections.Generic;
namespace EAN.GPD.Domain.Repositories
{
    internal class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> Filter(string whereExpression = null)
        {
            var objData = (TEntity)Activator.CreateInstance(typeof(TEntity), (long?)null);
            string where = string.IsNullOrWhiteSpace(whereExpression) ? "1 = 1" : whereExpression;
            var query = DatabaseProvider.NewQuery($"select {objData.GetNamePrimaryKey()} id from {objData.GetNameTable()} where {where}");
            query.ExecuteQuery();
            var result = new List<TEntity>();

            query.ForEach(() =>
            {
                var obj = Activator.CreateInstance(typeof(TEntity), query.GetLong("ID"));
                result.Add((TEntity)obj);
            });

            return result;
        }

        public IEnumerable<TEntity> Filter(uint page = 0, uint count = 10, string whereExpression = null)
        {
            string paginate = $"limit {count} offset ({page} * {count})";
            string where = string.IsNullOrWhiteSpace(whereExpression) ? $"1 = 1 {paginate}" : $"{whereExpression} {paginate}";
            return Filter(where);
        }

        public TEntity Find(string whereExpression)
        {
            if (string.IsNullOrWhiteSpace(whereExpression))
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var objData = (TEntity)Activator.CreateInstance(typeof(TEntity), (long?)null);
            var query = DatabaseProvider.NewQuery($"select {objData.GetNamePrimaryKey()} id from {objData.GetNameTable()} where {whereExpression} limit 1");
            query.ExecuteQuery();
            if (query.IsNotEmpty)
            {
                return (TEntity)Activator.CreateInstance(typeof(TEntity), query.GetLong("ID"));
            }

            return null;
        }

        public TEntity GetOne(long id) => (TEntity)Activator.CreateInstance(typeof(TEntity), id);
    }
}
