using EAN.GPD.Infrastructure.Database;

namespace EAN.GPD.Domain.Entities
{
    public enum TipoArvore
    {
        Projeto = 1,
        Departamento = 2,
        Grupo = 3,
        Cargo = 4,
        Usuario = 5,
        Indicador = 6,
        Corporativo = 7
    }

    public class ArvoreEntity : BaseEntity
    {
        public ArvoreEntity(long? idArvore = null) : base("Arvore", idArvore) {}

        [Column]
        public long IdArvore { get; set; }

        [Column(StringMaxLenght = 255, StringNotNullable = true)]
        public string Descricao { get; set; }

        [Column]
        public TipoArvore TipoArvore { get; set; }

        [Column]
        public long IdProjeto { get; set; }

        [Column]
        public long? IdIndicador { get; set; }

        [Column]
        public long? IdDepartamento { get; set; }

        [Column]
        public long? IdUsuarioGrupo { get; set; }

        [Column]
        public long? IdCargo { get; set; }

        [Column]
        public long? IdUsuario { get; set; }

        [Column(StringMaxLenght = 255)]
        public string NomeIndicador { get; set; }

        [Column]
        public decimal? ValorPercentualPeso { get; set; }

        [Column]
        public decimal? ValorPercentualCriterio { get; set; }

        [Column]
        public long? IdArvoreSuperior { get; set; }

        [JoinColumn("IdArvoreSuperior")]
        public ArvoreEntity ArvoreSuperior { get; private set; }

        [JoinColumn("IdProjeto")]
        public ProjetoEntity Projeto { get; private set; }

        [JoinColumn("IdIndicador")]
        public IndicadorEntity Indicador { get; private set; }

        [JoinColumn("IdDepartamento")]
        public DepartamentoEntity Departamento { get; private set; }

        [JoinColumn("IdUsuarioGrupo")]
        public UsuarioGrupoEntity UsuarioGrupo { get; private set; }

        [JoinColumn("IdUsuario")]
        public UsuarioEntity Usuario { get; private set; }

        [JoinColumn("IdCargo")]
        public CargoEntity Cargo { get; private set; }
    }
}
