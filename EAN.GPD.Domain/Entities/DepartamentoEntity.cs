using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public class DepartamentoEntity : BaseEntity
    {
        private const string tableName = "Departamento";
        public DepartamentoEntity(long idUsuario) : base(tableName, idUsuario) { }
        public DepartamentoEntity() : base(tableName) { }

        [Column]
        public long IdDepartamento { get; set; }

        [Column]
        public int Codigo { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }
    }
}
