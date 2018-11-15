using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAN.GPD.Domain.Models
{
    public class UsuarioModel : BaseModel
    {
        [NotMapped]
        public long? IdUsuario { get; set; }

        public override long? GetId() => IdUsuario;

        [Required(AllowEmptyStrings = false, ErrorMessage = "O login do usuário é obrigatório.")]
        [StringLength(maximumLength: 30, ErrorMessage = "O login do usuário não pode conter mais do que 30 caracteres.")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O nome do usuário é obrigatório.")]
        [StringLength(maximumLength: 30, ErrorMessage = "O nome do usuário não pode conter mais do que 150 caracteres.")]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo ativo do usuário é obrigatório.")]
        public bool Ativo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo administrador do usuário é obrigatório.")]
        public bool Administrador { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O ID de usuário grupo é obrigatório.")]
        [Range(minimum: 1, maximum: long.MaxValue,ErrorMessage = "O ID de usuário grupo é inválido.")]
        public long IdUsuarioGrupo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A senha do usuário é obrigatório.")]
        public string SenhaLogin { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(maximumLength: 11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 caracteres.")]
        public string Cpf { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O email do usuário é obrigatório.")]
        [StringLength(maximumLength: 255, ErrorMessage = "O email do usuário não pode conter mais do que 255 caracteres.")]
        public string Email { get; set; }

        [StringLength(maximumLength: 30, ErrorMessage = "A matrícula do usuário não pode conter mais do que 30 caracteres.")]
        public string Matricula { get; set; }

        public decimal? ValorPesoIndividual { get; set; }

        public decimal? ValorPesoCorporativo { get; set; }
    }
}
