using EAN.GPD.Domain.Entities;
using System.Collections.Generic;

namespace EAN.GPD.Domain.Utils
{
    public struct ResultSet<TEntity> where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> Data { get; private set; }
        public int Count { get; private set; }

        public ResultSet(IEnumerable<TEntity> data, int count)
        {
            Data = data;
            Count = count;
        }
    }
}
