using EAN.GPD.Infrastructure.Database;
using System;

namespace EAN.GPD.Domain.Entities
{
    public class ProjetoEntity : BaseEntity
    {
        private const string tableName = "Projeto";
        public ProjetoEntity(long idUsuario) : base(tableName, idUsuario) { }
        public ProjetoEntity() : base(tableName) { }

        [Column]
        public long IdProjeto { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Nome { get; set; }

        [Column]
        public bool Ativo { get; set; }

        [Column]
        public DateTime DataInicio { get; set; }

        [Column]
        public DateTime DataTermino { get; set; }
    }
}
