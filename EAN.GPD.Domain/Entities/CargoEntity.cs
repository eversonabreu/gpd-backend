using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class CargoEntity : BaseEntity
    {
        private const string tableName = "Cargo";
        public CargoEntity(long idUsuario) : base(tableName, idUsuario) {}
        public CargoEntity() : base(tableName) {}

        [Column]
        public long IdCargo { get; set; }

        [Column]
        public int Codigo { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }
    }
}
