using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class CargoEntity : BaseEntity
    {
        public CargoEntity(long? idCargo = null) : base("Cargo", idCargo) {}

        [Column]
        public long IdCargo { get; set; }

        [Column]
        public int Codigo { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }
    }
}
