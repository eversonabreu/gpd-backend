using System.Collections.Generic;

namespace EAN.GPD.Domain.Dto
{
    public struct ResultadoCalculo
    {
        public decimal ValorMeta { get; set; }
        public decimal ValorRealizado { get; set; }
        public decimal ValorAtingimento { get; set; }
        public string Identificador { get; set; }
        public string Cardinalidade { get; set; }
        public string UnidadeMedida { get; set; }
        public string UsuarioResponsavel { get; set; }
        public decimal PesoPorAtingimento { get; set; }
    }

    public class ResultadoCalculoAgregado
    {
        public List<ResultadoCalculo> Resultados { get; set; }
        public decimal ValorPonderadoIndividualPessoa { get; set; }
        public decimal ValorPonderadoCorporativo { get; set; }
        public decimal ValorPonderadoFinalPessoa { get; set; }

        public ResultadoCalculoAgregado() => Resultados = new List<ResultadoCalculo>();
    }
}
