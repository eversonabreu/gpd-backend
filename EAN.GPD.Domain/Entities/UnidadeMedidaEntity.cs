using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class UnidadeMedidaEntity : BaseEntity
    {
        private const string tableName = "UnidadeMedida";
        public UnidadeMedidaEntity(long idUsuario) : base(tableName, idUsuario) { }
        public UnidadeMedidaEntity() : base(tableName) { }

        [Column]
        public long IdUnidadeMedida { get; set; }

        [Column]
        public int Codigo { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }
    }
}
