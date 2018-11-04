using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface IDepartamentoRepository : IBaseRepository<DepartamentoEntity>
    {
    }

    internal class DepartamentoRepository : BaseRepository<DepartamentoEntity>, IDepartamentoRepository
    {
    }
}
