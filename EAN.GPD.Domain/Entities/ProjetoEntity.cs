using EAN.GPD.Infrastructure.Database;
using System;

namespace EAN.GPD.Domain.Entities
{
    public class ProjetoEntity : BaseEntity
    {
        public ProjetoEntity(long? idProjeto = null) : base("Projeto", idProjeto) {}

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
