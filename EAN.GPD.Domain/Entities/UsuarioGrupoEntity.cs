using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class UsuarioGrupoEntity : BaseEntity
    {
        public UsuarioGrupoEntity(long? idUsuarioGrupo = null) : base("UsuarioGrupo", idUsuarioGrupo) {}

        [Column]
        public long IdUsuarioGrupo { get; set; }

        [Column]
        public int Codigo { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }
    }
}
