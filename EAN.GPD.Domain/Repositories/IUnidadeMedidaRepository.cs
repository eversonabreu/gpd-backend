using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface IUnidadeMedidaRepository : IBaseRepository<UnidadeMedidaEntity>
    {
    }

    internal class UnidadeMedidaRepository : BaseRepository<UnidadeMedidaEntity>, IUnidadeMedidaRepository
    {
    }
}
