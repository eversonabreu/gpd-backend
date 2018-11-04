using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class DepartamentoEntity : BaseEntity
    {
        public DepartamentoEntity(long? idDepartamento = null) : base("Departamento", idDepartamento) {}

        [Column]
        public long IdDepartamento { get; set; }

        [Column]
        public int Codigo { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }
    }
}
