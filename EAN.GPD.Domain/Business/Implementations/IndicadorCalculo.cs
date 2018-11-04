using EAN.GPD.Domain.Business.Interfaces;
using EAN.GPD.Domain.Dto;
using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Repositories;
using EAN.GPD.Infrastructure.Database;
using System;
using System.Linq;
using System.Threading;

namespace EAN.GPD.Domain.Business.Implementations
{
    internal class IndicadorCalculo : IIndicadorCalculo
    {
        private readonly IIndicadorRepository indicadorRepository;
        private readonly IArvoreRepository arvoreRepository;

        public IndicadorCalculo(IIndicadorRepository indicadorRepository,
            IArvoreRepository arvoreRepository)
        {
            this.indicadorRepository = indicadorRepository;
            this.arvoreRepository = arvoreRepository;
        }

        private decimal ObterValorMetaMovimento(long idProjeto, IndicadorEntity indicadorEntity, DateTime dataInicial, DateTime dataFinal)
        {
            string tipoOperacao = indicadorEntity.TipoAcumuloMeta == TipoAcumulo.Media ? "avg" : "sum";
            string sql = $"select round(coalesce({tipoOperacao}(ValorMeta), 0), 2) valor from Movimento where IdProjeto = {idProjeto} and IdIndicador = {indicadorEntity.IdIndicador} ";
            if (indicadorEntity.TipoAcumuloMeta == TipoAcumulo.NaoAcumulavel)
            {
                sql += "and DataLancamento = :P1";
                var database = DatabaseProvider.NewQuery(sql);
                database.AddParameter("P1", dataInicial.Date);
                database.ExecuteQuery();
                return database.GetDecimal("VALOR");
            }
            else
            {
                sql += "and DataLancamento Between :P1 and :P2";
                var database = DatabaseProvider.NewQuery(sql);
                database.AddParameter("P1", dataInicial.Date);
                database.AddParameter("P2", dataFinal.Date);
                database.ExecuteQuery();
                return database.GetDecimal("VALOR");
            }
        }

        private decimal ObterValorRealizadoMovimento(long idProjeto, IndicadorEntity indicadorEntity, DateTime dataInicial, DateTime dataFinal)
        {
            string tipoOperacao = indicadorEntity.TipoAcumuloRealizado == TipoAcumulo.Media ? "avg" : "sum";
            string sql = $"select round(coalesce({tipoOperacao}(ValorRealizado), 0), 2) valor from Movimento where IdProjeto = {idProjeto} and IdIndicador = {indicadorEntity.IdIndicador} ";
            if (indicadorEntity.TipoAcumuloRealizado == TipoAcumulo.NaoAcumulavel)
            {
                sql += "and DataLancamento = :P1";
                var database = DatabaseProvider.NewQuery(sql);
                database.AddParameter("P1", dataInicial.Date);
                database.ExecuteQuery();
                return database.GetDecimal("VALOR");
            }
            else
            {
                sql += "and DataLancamento Between :P1 and :P2";
                var database = DatabaseProvider.NewQuery(sql);
                database.AddParameter("P1", dataInicial.Date);
                database.AddParameter("P2", dataFinal.Date);
                database.ExecuteQuery();
                return database.GetDecimal("VALOR");
            }
        }

        private decimal ObterValorAtingimento(IndicadorEntity indicadorEntity, decimal valorMeta, decimal valorRealizado)
        {
            decimal diferencaRealizadoMeta = valorRealizado - valorMeta;
            decimal diferencaSobreMeta = diferencaRealizadoMeta / valorMeta;
            decimal pesoSobreCriterio = indicadorEntity.ValorPercentualPeso / indicadorEntity.ValorPercentualCriterio;
            decimal produto = diferencaSobreMeta * pesoSobreCriterio;
            decimal atingimento = produto * 100;

            if (indicadorEntity.TipoCardinalidade == TipoCardinalidade.QuantoMaiorMelhor)
            {
                atingimento = (1 + produto) * 100;
            }
            else if (indicadorEntity.TipoCardinalidade == TipoCardinalidade.QuantoMenorMelhor)
            {
                atingimento = (1 - produto) * 100;
            }

            if (indicadorEntity.ValorMinimoAtingimento.HasValue && atingimento < indicadorEntity.ValorMinimoAtingimento.Value)
            {
                return indicadorEntity.ValorMinimoAtingimento.Value;
            }
            else if (indicadorEntity.ValorMaximoAtingimento.HasValue && atingimento > indicadorEntity.ValorMaximoAtingimento.Value)
            {
                return indicadorEntity.ValorMaximoAtingimento.Value;
            }

            return atingimento;
        }

        private ushort ObterQuantidadeCaracteresAteOFimDaExpressa(string formula, int posicaoInicial)
        {
            ushort quantidade = 0;
            for (int index = posicaoInicial + 1; index < formula.Length; index++)
            {
                quantidade++;
                if (formula[index] == ']')
                {
                    return quantidade;
                }
            }

            throw new Exception($"Formula inválida! '{formula}'.");
        }

        private decimal ObterValorFormula(string formula)
        {
            var query = DatabaseProvider.NewQuery($"select ({formula}) valor");
            query.ExecuteQuery();
            return query.GetDecimal("VALOR");
        }

        private decimal ObterValorCalculado(IndicadorEntity indicador, long idProjeto, DateTime dataInicial, DateTime dataFinal, bool meta = true)
        {
            if (indicador.Formula is null)
            {
                throw new ArgumentNullException($"A fórmuala do indicador: '{indicador.Identificador}' não foi definida.");
            }

            string formula = new string(indicador.Formula);
            int posicaoInicial = -1;
            while ((posicaoInicial = formula.IndexOf('[')) != -1)
            {
                ushort quantidadeCaracteresAteOFim = ObterQuantidadeCaracteresAteOFimDaExpressa(indicador.Formula, posicaoInicial);
                string indicadorFormula = indicador.Formula.Substring(posicaoInicial, quantidadeCaracteresAteOFim).Replace("[", "").Replace("]", "");
                
                if (string.IsNullOrWhiteSpace(indicadorFormula))
                {
                    throw new Exception($"Fórmula inválida. '{indicador.Formula}'.");
                }

                var novoIndicador = indicadorRepository.Find($"Identificador = '{indicadorFormula}'");
                if (novoIndicador is null)
                {
                    throw new Exception($"Não foi possível localizar um indicador com este identificador: '{indicadorFormula}'.");
                }

                decimal valor = meta ? ObterValorMetaMovimento(idProjeto, novoIndicador, dataInicial, dataFinal) : ObterValorRealizadoMovimento(idProjeto, novoIndicador, dataInicial, dataFinal);
                string valorFormatado = valor.ToString().Replace(".", "").Replace(",", ".");
                formula = formula.Replace($"[{indicadorFormula}]", valorFormatado);
            }

            return ObterValorFormula(formula);
        }

        private ResultadoCalculo ObterResultadosCalculados(IndicadorEntity indicador, long idProjeto, DateTime dataInicial, DateTime dataFinal)
        {
            var resultadoCalculo = new ResultadoCalculo
            {
                ValorMeta = ObterValorCalculado(indicador, idProjeto, dataInicial, dataFinal),
                ValorRealizado = ObterValorCalculado(indicador, idProjeto, dataInicial, dataFinal, false)
            };

            resultadoCalculo.ValorAtingimento = ObterValorAtingimento(indicador, resultadoCalculo.ValorMeta, resultadoCalculo.ValorRealizado);
            DefinirPropriedadesIndicador(indicador, resultadoCalculo);
            return resultadoCalculo;
        }

        public ResultadoCalculo ObterResultados(long idProjeto, long idIndicador, DateTime dataInicial, DateTime dataFinal)
        {
            var resultadoCalculo = new ResultadoCalculo();
            var indicador = indicadorRepository.GetOne(idIndicador);
            bool calcularRealizado = true;
            bool calcularMeta = true;

            if (indicador.TipoCalculo != TipoCalculo.NaoCalculado)
            {
                if (indicador.TipoCalculo == TipoCalculo.Ambos)
                {
                    return ObterResultadosCalculados(indicador, idProjeto, dataInicial, dataFinal);
                }
                else if (indicador.TipoCalculo == TipoCalculo.SomenteMeta)
                {
                    resultadoCalculo.ValorMeta = ObterValorCalculado(indicador, idProjeto, dataInicial, dataFinal);
                    calcularMeta = false;
                }
                else
                {
                    resultadoCalculo.ValorRealizado = ObterValorCalculado(indicador, idProjeto, dataInicial, dataFinal, false);
                    calcularRealizado = false;
                }
            }
            
            if (calcularMeta)
            {
                resultadoCalculo.ValorMeta = ObterValorMetaMovimento(idProjeto, indicador, dataInicial, dataFinal);
            }

            if (calcularRealizado)
            {
                resultadoCalculo.ValorRealizado = ObterValorRealizadoMovimento(idProjeto, indicador, dataInicial, dataFinal);
            }

            resultadoCalculo.ValorAtingimento = ObterValorAtingimento(indicador, resultadoCalculo.ValorMeta, resultadoCalculo.ValorRealizado);
            DefinirPropriedadesIndicador(indicador, resultadoCalculo);
            return resultadoCalculo;
        }

        private void DefinirPropriedadesIndicador(IndicadorEntity indicador, ResultadoCalculo resultado)
        {
            resultado.Identificador = indicador.Identificador;
            resultado.UnidadeMedida = $"{indicador.UnidadeMedida.Codigo} - {indicador.UnidadeMedida.Descricao}";
            resultado.UsuarioResponsavel = indicador.UsuarioResponsavel.Nome;
            switch (indicador.TipoCardinalidade)
            {
                case TipoCardinalidade.Exata: resultado.Cardinalidade = "Exata"; break;
                case TipoCardinalidade.QuantoMaiorMelhor: resultado.Cardinalidade = "Quanto maior melhor"; break;
                case TipoCardinalidade.QuantoMenorMelhor: resultado.Cardinalidade = "Quanto menor melhor"; break;
                default: throw new Exception($"Tipo de cardinalidade inválida no indicador: '{indicador.Identificador}'.");
            }
        }

        private void ObterResultadosCalculosThread(object obj)
        {
            var tuple = (Tuple<ResultadoCalculoAgregado, decimal, long, long, DateTime, DateTime>)obj;
            var result = ObterResultados(tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6);
            result.PesoPorAtingimento = result.ValorAtingimento * tuple.Item2;
            tuple.Item1.Resultados.Add(result);
        }

        public ResultadoCalculoAgregado ObterResultadosCalculosPorIndicadoresCorporativos(long idArvore, DateTime dataInicial, DateTime dataFinal)
        {
            var result = new ResultadoCalculoAgregado();
            var query = DatabaseProvider.NewQuery($"select a.IdProjeto, a.IdIndicador, a.ValorPercentualPeso from Arvore a where a.IdProjeto = (select IdProjeto from Arvore where IdArvore = {idArvore}) and a.TipoArvore = {(int)TipoArvore.Corporativo}");
            query.ExecuteQuery();
            decimal somaPesos = 0;
            query.ForEach(() =>
            {
                var obj = new Tuple<ResultadoCalculoAgregado, decimal, long, long, DateTime, DateTime>(result, query.GetDecimal("VALORPERCENTUALPESO"), query.GetLong("IDPROJETO"), query.GetLong("IDINDICADOR"), dataInicial, dataFinal);
                var thread = new Thread(new ParameterizedThreadStart(ObterResultadosCalculosThread));
                thread.Start(obj);
                somaPesos += query.GetDecimal("VALORPERCENTUALPESO");
            });

            while (result.Resultados.Count < query.Count) continue;

            result.ValorPonderadoCorporativo = decimal.Round(result.Resultados.Sum(item => item.PesoPorAtingimento) / somaPesos, 2);
            return result;
        }

        public ResultadoCalculoAgregado ObterResultadosCalculosPorPessoa(long idArvore, DateTime dataInicial, DateTime dataFinal)
        {
            var arvores = arvoreRepository.Filter($"IdArvoreSuperior = {idArvore} and TipoArvore = {(int) TipoArvore.Indicador}");
            decimal somaPesos = 0;
            var result = new ResultadoCalculoAgregado();
            if (arvores.Any())
            {
                foreach (var arvore in arvores)
                {
                    var obj = new Tuple<ResultadoCalculoAgregado, decimal, long, long, DateTime, DateTime>(result, arvore.ValorPercentualPeso.Value, arvore.IdProjeto, arvore.IdIndicador.Value, dataInicial, dataFinal);
                    var thread = new Thread(new ParameterizedThreadStart(ObterResultadosCalculosThread));
                    thread.Start(obj);
                    somaPesos += arvore.ValorPercentualPeso.Value;
                }

                while (result.Resultados.Count < arvores.Count()) continue;

                result.ValorPonderadoIndividualPessoa = decimal.Round(result.Resultados.Sum(item => item.PesoPorAtingimento) / somaPesos, 2);
                result.ValorPonderadoCorporativo = ObterResultadosCalculosPorIndicadoresCorporativos(idArvore, dataInicial, dataFinal).ValorPonderadoCorporativo;

                long idUsuario = arvores.First().IdUsuario.Value;
                var queryPesosUsuario = DatabaseProvider.NewQuery($"select coalesce(ValorPesoIndividual, 0 ) vpi, coalesce(ValorPesoCorporativo, 0) vpc, ValorMinimoPonderado, ValorMaximoPonderado from Usuario where IdUsuario = {idUsuario}");
                queryPesosUsuario.ExecuteQuery();

                if (!queryPesosUsuario.IsNull("VALORMINIMOPONDERADO") && result.ValorPonderadoIndividualPessoa < queryPesosUsuario.GetDecimal("VALORMINIMOPONDERADO"))
                {
                    result.ValorPonderadoIndividualPessoa = queryPesosUsuario.GetDecimal("VALORMINIMOPONDERADO");
                }
                else if (!queryPesosUsuario.IsNull("VALORMAXIMOPONDERADO") && result.ValorPonderadoIndividualPessoa > queryPesosUsuario.GetDecimal("VALORMAXIMOPONDERADO"))
                {
                    result.ValorPonderadoIndividualPessoa = queryPesosUsuario.GetDecimal("VALORMAXIMOPONDERADO");
                }

                result.ValorPonderadoFinalPessoa = decimal.Round(result.ValorPonderadoIndividualPessoa * (queryPesosUsuario.GetDecimal("VPI") / 100) + (result.ValorPonderadoCorporativo * (queryPesosUsuario.GetDecimal("VPC") / 100)), 2);
            }

            return result;
        }
    }
}
