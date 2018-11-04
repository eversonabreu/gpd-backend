using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface IProjetoRepository : IBaseRepository<ProjetoEntity>
    {
    }

    internal class ProjetoRepository : BaseRepository<ProjetoEntity>, IProjetoRepository
    {
    }
}
