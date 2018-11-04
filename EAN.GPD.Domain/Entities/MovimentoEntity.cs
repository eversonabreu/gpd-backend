using EAN.GPD.Infrastructure.Database;
using System;

namespace EAN.GPD.Domain.Entities
{
    public class MovimentoEntity : BaseEntity
    {
        public MovimentoEntity(long? idMovimento = null) : base("Movimento", idMovimento) {}

        [Column]
        public long IdMovimento { get; set; }

        [Column]
        public long IdProjeto { get; set; }

        [Column]
        public long IdIndicador { get; set; }

        [Column]
        public DateTime DataLancamento { get; set; }

        [Column]
        public decimal ValorMeta { get; set; }

        [Column]
        public decimal ValorRealizado { get; set; }

        [JoinColumn("IdProjeto")]
        public ProjetoEntity Projeto { get; }

        [JoinColumn("IdIndicador")]
        public IndicadorEntity Indicador { get; }
    }
}
