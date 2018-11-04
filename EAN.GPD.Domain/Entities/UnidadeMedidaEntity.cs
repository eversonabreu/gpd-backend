using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class UnidadeMedidaEntity : BaseEntity
    {
        public UnidadeMedidaEntity(long? idUnidadeMedida = null) : base("UnidadeMedida", idUnidadeMedida) {}

        [Column]
        public long IdUnidadeMedida { get; set; }

        [Column]
        public int Codigo { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }
    }
}
