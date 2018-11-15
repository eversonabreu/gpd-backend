using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAN.GPD.Domain.Models
{
    public class UsuarioGrupoModel : BaseModel
    {
        [NotMapped]
        public long? IdUsuarioGrupo { get; set; }

        public override long? GetId() => IdUsuarioGrupo;

        [Required(AllowEmptyStrings = false, ErrorMessage = "O código do grupo de usuário é obrigatório.")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Código de grupo de usuário inválido.")]
        public int Codigo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A descrição do grupo de usuário é obrigatório.")]
        [StringLength(maximumLength: 255, ErrorMessage = "A descrição do grupo de usuário não pode conter mais do que 255 caracteres.")]
        public string Descricao { get; set; }
    }
}
