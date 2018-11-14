using System;
using System.ComponentModel.DataAnnotations;

namespace EAN.GPD.Domain.Models
{
    public class MovimentoModel : BaseModel
    {
        public long? IdMovimento { get; set; }

        public override long? GetId() => IdMovimento;

        [Required(AllowEmptyStrings = false, ErrorMessage = "O ID de Projeto é obrigatório.")]
        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "O ID de Projeto é inválido.")]
        public long IdProjeto { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O ID de Indicador é obrigatório.")]
        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "O ID de Indicador é inválido.")]
        public long IdIndicador { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data de Lançamento é obrigatório.")]
        public DateTime DataLancamento { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Valor da meta é obrigatório.")]
        public decimal ValorMeta { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Valor do realizado é obrigatório.")]
        public decimal ValorRealizado { get; set; }
    }
}
