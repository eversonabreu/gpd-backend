using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class UsuarioEntity : BaseEntity
    {
        public UsuarioEntity(long? idUsuario = null) : base("Usuario", idUsuario) {}

        [Column]
        public long IdUsuario { get; set; }

        [Column(StringMaxLenght = 30, StringNotNullable = true)]
        public string Login { get; set; }

        [Column(StringMaxLenght = 150, StringNotNullable = true)]
        public string Nome { get; set; }

        [Column]
        public bool Ativo { get; set; }

        [Column]
        public bool Administrador { get; set; }

        [Column]
        public long IdUsuarioGrupo { get; set; }

        [JoinColumn("IdUsuarioGrupo")]
        public UsuarioGrupoEntity UsuarioGrupo { get; private set; }

        [Column(StringNotNullable = true)]
        public string SenhaLogin { get; set; }

        [Column(StringMaxLenght = 11, StringNotNullable = true)]
        public string Cpf { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Email { get; set; }

        [Column(StringMaxLenght = 30)]
        public string Matricula { get; set; }

        [Column]
        public decimal? ValorPesoIndividual { get; set; }

        [Column]
        public decimal? ValorPesoCorporativo { get; set; }
    }
}
