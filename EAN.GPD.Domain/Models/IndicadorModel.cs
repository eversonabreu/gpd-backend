using System.ComponentModel.DataAnnotations;

namespace EAN.GPD.Domain.Models
{
    public class IndicadorModel : BaseModel
    {
        public long? IdIndicador { get; set; }

        public override long? GetId() => IdIndicador;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo ativo é obrigatório.")]
        public bool Ativo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O identificador é obrigatório.")]
        [StringLength(maximumLength: 10, ErrorMessage = "O identificador não pode conter mais do que 10 caracteres.")]
        public string Identificador { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O nome é obrigatório.")]
        [StringLength(maximumLength: 100, ErrorMessage = "O nome não pode conter mais do que 100 caracteres.")]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O percentual de peso é obrigatório.")]
        [Range(minimum: 0d, maximum: 100d, ErrorMessage = "Valor para o percentual do peso deve estar entre '0,00' e '100,00'.")]
        public decimal ValorPercentualPeso { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O tipo de remuneração é obrigatório.")]
        [Range(minimum: 1, maximum: 4, ErrorMessage = "Valor inválido para o tipo de remuneração. Valores válidos: 'Não remunerado = 1, Anual = 2, Mensal = 3, Diário = 4'.")]
        public int TipoRemuneracao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O tipo de cardinalidade é obrigatório.")]
        [Range(minimum: 1, maximum: 4, ErrorMessage = "Valor inválido para o tipo de cardinalidade. Valores válidos: 'Exata = 1, Quanto maior melhor = 2, Quanto menor melhor = 3'.")]
        public int TipoCardinalidade { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O tipo de acúmulo da meta é obrigatório.")]
        [Range(minimum: 1, maximum: 4, ErrorMessage = "Valor inválido para o tipo de acúmulo de meta. Valores válidos: 'Não acumulável = 1, Acumulável = 2, Média = 3'.")]
        public int TipoAcumuloMeta { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O tipo de acúmulo do realizado é obrigatório.")]
        [Range(minimum: 1, maximum: 4, ErrorMessage = "Valor inválido para o tipo de acúmulo do realizado. Valores válidos: 'Não acumulável = 1, Acumulável = 2, Média = 3'.")]
        public int TipoAcumuloRealizado { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O 'ID de Unidade de Medida' é obrigatório.")]
        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdUnidadeMedida'.")]
        public long IdUnidadeMedida { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O 'ID de Usuário Responsável' é obrigatório.")]
        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdUsuarioResponsavel'.")]
        public long IdUsuarioResponsavel { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O tipo de periodicidade é obrigatório.")]
        [Range(minimum: 1, maximum: 4, ErrorMessage = "Valor inválido para o tipo de periodicidade. Valores válidos: 'Diário = 1, Mensal = 2, Anual = 3'.")]
        public int TipoPeriodicidade { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo 'corporativo' é obrigatório.")]
        public bool Corporativo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O percentual de critério é obrigatório.")]
        [Range(minimum: 0d, maximum: 100d, ErrorMessage = "Valor para o percentual do critério deve estar entre '0,00' e '100,00'.")]
        public decimal ValorPercentualCriterio { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O tipo de cálculo é obrigatório.")]
        [Range(minimum: 1, maximum: 4, ErrorMessage = "Valor inválido para o tipo de cálculo. Valores válidos: 'Não calculado = 1, Somente meta = 2, Somente realizado = 3, Ambos = 4'.")]
        public int TipoCalculo { get; set; }

        public string Formula { get; set; }

        public string Observacao { get; set; }

        public decimal? ValorMinimoAtingimento { get; set; }

        public decimal? ValorMaximoAtingimento { get; set; }

        public decimal? ValorMinimoPonderado { get; set; }

        public decimal? ValorMaximoPonderado { get; set; }
    }
}
