using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface IIndicadorRepository : IBaseRepository<IndicadorEntity>
    {
    }

    internal class IndicadorRepository : BaseRepository<IndicadorEntity>, IIndicadorRepository
    {
    }
}
