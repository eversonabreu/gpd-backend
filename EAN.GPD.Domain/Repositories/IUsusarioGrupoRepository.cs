using EAN.GPD.Domain.Entities;

namespace EAN.GPD.Domain.Repositories
{
    public interface IUsuarioGrupoRepository : IBaseRepository<UsuarioGrupoEntity>
    {
    }

    internal class UsuarioGrupoRepository : BaseRepository<UsuarioGrupoEntity>, IUsuarioGrupoRepository
    {
    }
}
