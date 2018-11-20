using EAN.GPD.Infrastructure.Database;
using System;

namespace EAN.GPD.Domain.Entities
{
    public class MovimentoEntity : BaseEntity
    {
        private const string tableName = "Movimento";
        public MovimentoEntity(long idUsuario) : base(tableName, idUsuario) { }
        public MovimentoEntity() : base(tableName) { }

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
        public ProjetoEntity Projeto { get; private set; }

        [JoinColumn("IdIndicador")]
        public IndicadorEntity Indicador { get; private set; }
    }
}
