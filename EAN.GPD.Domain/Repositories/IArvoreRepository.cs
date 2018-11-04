using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface IArvoreRepository : IBaseRepository<ArvoreEntity>
    {
    }

    internal class ArvoreRepository : BaseRepository<ArvoreEntity>, IArvoreRepository
    {
    }
}
