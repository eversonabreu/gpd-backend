using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class UsuarioGrupoEntity : BaseEntity
    {
        private const string tableName = "UsuarioGrupo";
        public UsuarioGrupoEntity(long idUsuario) : base(tableName, idUsuario) { }
        public UsuarioGrupoEntity() : base(tableName) { }

        [Column]
        public long IdUsuarioGrupo { get; set; }

        [Column]
        public int Codigo { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }
    }
}
