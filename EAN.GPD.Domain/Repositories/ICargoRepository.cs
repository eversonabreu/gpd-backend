using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface ICargoRepository : IBaseRepository<CargoEntity>
    {
    }

    internal class CargoRepository : BaseRepository<CargoEntity>, ICargoRepository
    {
    }
}
