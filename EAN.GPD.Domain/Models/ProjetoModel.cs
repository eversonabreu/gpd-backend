using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAN.GPD.Domain.Models
{
    public class ProjetoModel : BaseModel
    {
        [NotMapped]
        public long? IdProjeto { get; set; }

        public override long? GetId() => IdProjeto;

        [Required(AllowEmptyStrings = false, ErrorMessage = "O nome do projeto é obrigatório.")]
        [StringLength(maximumLength: 255, ErrorMessage = "O nome do projeto não pode conter mais do que 255 caracteres.")]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ativo é obrigatório.")]
        public bool Ativo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data de início é obrigatório.")]
        public DateTime DataInicio { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data de término é obrigatório.")]
        public DateTime DataTermino { get; set; }
    }
}
