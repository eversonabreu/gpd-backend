using EAN.GPD.Infrastructure.Database;
using System.ComponentModel;

namespace EAN.GPD.Domain.Entities
{
    public enum TipoRemuneracao
    {
        [Description("Não remunerado")]
        NaoRemunerado = 1,
        [Description("Anual")]
        Anual = 2,
        [Description("Mensal")]
        Mensal = 3,
        [Description("Diário")]
        Diario = 4
    }

    public enum TipoCardinalidade
    {
        [Description("Exata")]
        Exata = 1,
        [Description("Quanto maior melhor")]
        QuantoMaiorMelhor = 2,
        [Description("Quanto menor melhor")]
        QuantoMenorMelhor = 3
    }

    public enum TipoAcumulo
    {
        [Description("Não acumulável")]
        NaoAcumulavel = 1,
        [Description("Acumulável")]
        Acumulavel = 2,
        [Description("Média")]
        Media = 3
    }

    public enum TipoPeriodicidade
    {
        [Description("Diário")]
        Diario = 1,
        [Description("Mensal")]
        Mensal = 2,
        [Description("Anual")]
        Anual = 3
    }

    public enum TipoCalculo
    {
        [Description("Não calculado")]
        NaoCalculado = 1,
        [Description("Somente meta")]
        SomenteMeta = 2,
        [Description("Somente realizado")]
        SomenteRealizado = 3,
        [Description("Ambos")]
        Ambos = 4
    }

    public class IndicadorEntity : BaseEntity
    {
        public IndicadorEntity(long? idIndicador = null) : base("Indicador", idIndicador) {}

        [Column]
        public long IdIndicador { get; set; }

        [Column]
        public bool Ativo { get; set; }

        [Column(StringMaxLenght = 10, StringNotNullable = true)]
        public string Identificador { get; set; }

        [Column(StringMaxLenght = 100, StringNotNullable = true)]
        public string Nome { get; set; }

        [Column]
        public decimal ValorPercentualPeso { get; set; }

        [Column]
        public TipoRemuneracao TipoRemuneracao { get; set; }

        [Column]
        public TipoCardinalidade TipoCardinalidade { get; set; }

        [Column]
        public TipoAcumulo TipoAcumuloMeta { get; set; }

        [Column]
        public TipoAcumulo TipoAcumuloRealizado { get; set; }

        [Column]
        public long IdUnidadeMedida { get; set; }

        [JoinColumn("IdUnidadeMedida")]
        public UnidadeMedidaEntity UnidadeMedida { get; private set; }

        [Column]
        public long IdUsuarioResponsavel { get; set; }

        [JoinColumn("IdUsuarioResponsavel")]
        public UsuarioEntity UsuarioResponsavel { get; private set; }

        [Column]
        public TipoPeriodicidade TipoPeriodicidade { get; set; }

        [Column]
        public bool Corporativo { get; set; }

        [Column]
        public decimal ValorPercentualCriterio { get; set; }

        [Column]
        public TipoCalculo TipoCalculo { get; set; }

        [Column]
        public string Formula { get; set; }

        [Column]
        public string Observacao { get; set; }

        [Column]
        public decimal? ValorMinimoAtingimento { get; set; }

        [Column]
        public decimal? ValorMaximoAtingimento { get; set; }

        [Column]
        public decimal? ValorMinimoPonderado { get; set; }

        [Column]
        public decimal? ValorMaximoPonderado { get; set; }
    }
}
