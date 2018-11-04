using EAN.GPD.Domain.Dto;
using System;

namespace EAN.GPD.Domain.Business.Interfaces
{
    public interface IIndicadorCalculo
    {
        ResultadoCalculo ObterResultados(long idProjeto, long idIndicador, DateTime dataInicial, DateTime dataFinal);
        ResultadoCalculoAgregado ObterResultadosCalculosPorPessoa(long idArvore, DateTime dataInicial, DateTime dataFinal);
        ResultadoCalculoAgregado ObterResultadosCalculosPorIndicadoresCorporativos(long idArvore, DateTime dataInicial, DateTime dataFinal);
    }
}
