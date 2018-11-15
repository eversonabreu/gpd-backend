using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAN.GPD.Domain.Models
{
    public class DepartamentoModel : BaseModel
    {
        [NotMapped]
        public long? IdDepartamento { get; set; }

        public override long? GetId() => IdDepartamento;

        [Required(AllowEmptyStrings = false, ErrorMessage = "O código do departamento é obrigatório.")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Código de departamento inválido.")]
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A descrição do departamento é obrigatório.")]
        [StringLength(maximumLength: 255, ErrorMessage = "A descrição do departamento não pode conter mais do que 255 caracteres.")]
        public string Descricao { get; set; }
    }
}
