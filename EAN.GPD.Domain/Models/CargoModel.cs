using System.ComponentModel.DataAnnotations;

namespace EAN.GPD.Domain.Models
{
    public class CargoModel : BaseModel
    {
        public long? IdCargo { get; set; }

        public override long? GetId() => IdCargo;

        [Required(AllowEmptyStrings = false, ErrorMessage = "O código do cargo é obrigatório.")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Código de cargo inválido.")]
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A descrição do cargo é obrigatório.")]
        [StringLength(maximumLength: 255, ErrorMessage = "A descrição do cargo não pode conter mais do que 255 caracteres.")]
        public string Descricao { get; set; }
    }
}
