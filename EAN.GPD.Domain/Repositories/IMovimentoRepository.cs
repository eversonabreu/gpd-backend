using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface IMovimentoRepository : IBaseRepository<MovimentoEntity>
    {
    }

    internal class MovimentoRepository : BaseRepository<MovimentoEntity>, IMovimentoRepository
    {
    }
}
