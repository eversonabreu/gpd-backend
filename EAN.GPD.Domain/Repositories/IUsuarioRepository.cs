using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface IUsuarioRepository : IBaseRepository<UsuarioEntity>
    {
    }

    internal class UsuarioRepository : BaseRepository<UsuarioEntity>, IUsuarioRepository
    {
    }
}
