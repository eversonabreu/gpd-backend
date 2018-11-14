using System.ComponentModel.DataAnnotations;

namespace EAN.GPD.Domain.Models
{
    public class UnidadeMedidaModel : BaseModel
    {
        public long? IdUnidadeMedida { get; set; }

        public override long? GetId() => IdUnidadeMedida;

        [Required(AllowEmptyStrings = false, ErrorMessage = "O código da unidade de medida é obrigatório.")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Código da unidade de medida inválido.")]
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A descrição da unidade de medida é obrigatório.")]
        [StringLength(maximumLength: 255, ErrorMessage = "A descrição da unidade de medida não pode conter mais do que 255 caracteres.")]
        public string Descricao { get; set; }
    }
}
