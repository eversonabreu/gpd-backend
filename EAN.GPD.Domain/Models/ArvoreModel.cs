using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EAN.GPD.Domain.Models
{
    public class ArvoreModel : BaseModel
    {
        [NotMapped]
        public long? IdArvore { get; set; }

        public override long? GetId() => IdArvore;

        [Required(AllowEmptyStrings = false, ErrorMessage = "A descrição da árvore é obrigatória.")]
        [StringLength(maximumLength: 255, ErrorMessage = "A descrição da árvore não pode conter mais do que 255 caracteres.")]
        public string Descricao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O tipo da árvore é obrigatório.")]
        [Range(minimum: 1, maximum: 7, ErrorMessage = "Valor inválido para o tipo de árvore. Valores válidos: 'Projeto = 1, Departamento = 2, Grupo = 3, Cargo = 4, Usuário = 5, Indicador = 6, Corporativo = 7'.")]
        public int TipoArvore { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O 'ID de Projeto' é obrigatório.")]
        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdProjeto'.")]
        public long IdProjeto { get; set; }

        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdIndicador'.")]
        public long? IdIndicador { get; set; }

        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdDepartamento'.")]
        public long? IdDepartamento { get; set; }

        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdUsuarioGrupo'.")]
        public long? IdUsuarioGrupo { get; set; }

        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdCargo'.")]
        public long? IdCargo { get; set; }

        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdUsuario'.")]
        public long? IdUsuario { get; set; }

        [StringLength(maximumLength: 255, ErrorMessage = "O nome do indicador não pode conter mais do que 255 caracteres.")]
        public string NomeIndicador { get; set; }

        [Range(minimum: 0d, maximum: 100d, ErrorMessage = "Valor para o percentual do peso deve estar entre '0,00' e '100,00'.")]
        public decimal? ValorPercentualPeso { get; set; }

        [Range(minimum: 0d, maximum: 100d, ErrorMessage = "Valor para o percentual do critério deve estar entre '0,00' e '100,00'.")]
        public decimal? ValorPercentualCriterio { get; set; }

        [Range(minimum: 1, maximum: long.MaxValue, ErrorMessage = "Valor mínimo para o ID é 1 (um). 'IdArvoreSuperior'.")]
        public long? IdArvoreSuperior { get; set; }
    }
}
