using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IFeriasClienteAplicacao : IBaseAplicacao<FeriasCliente>
    {
        bool VerificaExistenciaBoletoMensalista(int idCliente, DateTime dataCompetencia);
        void SalvarFeriasCliente(List<FeriasClienteViewModel> listaFerias, Cliente clinteBase);
    }

    public class FeriasClienteAplicacao : BaseAplicacao<FeriasCliente, IFeriasClienteServico>, IFeriasClienteAplicacao
    {
        private readonly IFeriasClienteServico _feriasClienteServico;
        private readonly IClienteServico _clienteServico;
        private readonly IContratoMensalistaServico _contratoMensalistaServico;
        private readonly IContaCorrenteClienteServico _contaCorrenteClienteServico;

        public FeriasClienteAplicacao(IFeriasClienteServico feriasClienteServico,
                                        IClienteServico clienteServico,
                                        IContratoMensalistaServico contratoMensalistaServico,
                                        IContaCorrenteClienteServico contaCorrenteClienteServico)
        {
            _feriasClienteServico = feriasClienteServico;
            _clienteServico = clienteServico;
            _contratoMensalistaServico = contratoMensalistaServico;
            _contaCorrenteClienteServico = contaCorrenteClienteServico;
        }

        public bool VerificaExistenciaBoletoMensalista(int idCliente, DateTime dataCompetencia)
        {
            return _feriasClienteServico.VerificaExistenciaBoletoMensalista(idCliente, dataCompetencia);
        }

        public void SalvarFeriasCliente(List<FeriasClienteViewModel> listaFerias, Cliente clienteBase)
        {
            if (clienteBase == null || clienteBase.Id == 0)
                return;

            var datas = listaFerias.Where(x => x.DataCompetencia >= new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1))?
                                    .SelectMany(x => x.ListaDataCompetenciaPeriodo)?
                                    .Distinct()?
                                    .ToList();

            var idCliente = listaFerias?.FirstOrDefault()?.Cliente?.Id ?? clienteBase?.Id ?? 0;
            var cliente = _clienteServico.BuscarPorId(idCliente) ?? null;

            var listaPeriodosBloqueados = new Dictionary<DateTime, bool>();
            foreach (var dt in datas)
            {
                listaPeriodosBloqueados.Add(dt, VerificaExistenciaBoletoMensalista(cliente?.Id ?? 0, dt));
            }

            var listaFeriasRemovidas = new List<FeriasCliente>();
            var feriasClienteBase = BuscarPor(x => x.Cliente.Id == cliente.Id);
            foreach (var item in feriasClienteBase?.Where(x => x.DataCompetencia >= new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1))?.ToList())
            {
                if ((listaFerias == null || !listaFerias.Any())
                    || (listaFerias.All(x => x.Id != item.Id)
                        && listaPeriodosBloqueados.Any(x => !x.Value && x.Key == item.DataCompetencia)))
                {
                    ExcluirPorId(item.Id);
                    item.ValorFeriasCalculadaAnterior = item.ValorFeriasCalculada;
                    foreach (var detalhe in item.ListaFeriasClienteDetalhe)
                    {
                        detalhe.ValorFeriasCalculadaAnterior = detalhe.ValorFeriasCalculada;
                        detalhe.ValorFeriasCalculada = 0;
                    }
                    item.ValorFeriasCalculada = 0;
                    listaFeriasRemovidas.Add(item);
                }
            }

            foreach (var item in listaFerias)
            {
                if (listaPeriodosBloqueados.Any(x => !x.Value && x.Key == item.DataCompetencia))
                {
                    var ferias = item.Id <= 0 ? item.ToEntity() : _feriasClienteServico.BuscarPorId(item.Id);
                    ferias.Cliente = item.Id > 0 ? ferias.Cliente : cliente;
                    ferias.ContratoMensalista = item.Id > 0 && ferias.InutilizarTodasVagas ? ferias.ContratoMensalista : _contratoMensalistaServico.BuscarPorId(ferias.ContratoMensalista.Id);

                    if (ferias.ListaFeriasClienteDetalhe == null)
                        ferias.ListaFeriasClienteDetalhe = new List<FeriasClienteDetalhe>();
                    for (int i = 0; i < ferias.ListaDataCompetenciaPeriodo.Count; i++)
                    {
                        var itemData = ferias.ListaDataCompetenciaPeriodo[i];
                        if (!ferias.ListaFeriasClienteDetalhe.Any(x => x.DataCompetencia == itemData))
                        {
                            ferias.ListaFeriasClienteDetalhe.Add(new FeriasClienteDetalhe
                            {
                                DataInicio = i == 0 ? ferias.DataInicio : itemData.AddMonths(-1).Date,
                                DataFim = ferias.ListaDataCompetenciaPeriodo.Count == 1
                                            || i + 1 == ferias.ListaDataCompetenciaPeriodo.Count
                                                ? ferias.DataFim
                                                : new DateTime(itemData.AddMonths(-1).Year, itemData.AddMonths(-1).Month, DateTime.DaysInMonth(itemData.AddMonths(-1).Year, itemData.AddMonths(-1).Month)),
                                ValorFeriasCalculadaAnterior = ferias.ListaFeriasClienteDetalhe.Any()
                                                                && (ferias.ListaFeriasClienteDetalhe.FirstOrDefault(x => x.DataCompetencia == itemData)?.ValorFeriasCalculada ?? 0) > 0
                                                                    ? ferias.ListaFeriasClienteDetalhe.FirstOrDefault(x => x.DataCompetencia == itemData)?.ValorFeriasCalculada ?? 0
                                                                    : ferias.ListaFeriasClienteDetalhe.FirstOrDefault(x => x.DataCompetencia == itemData)?.ValorFeriasCalculadaAnterior ?? 0,
                                FeriasCliente = ferias
                            });
                        }
                    }

                    Salvar(ferias);
                }
            }

            var feriasSalvasBase = BuscarPor(x => x.Cliente.Id == cliente.Id)?.ToList() ?? new List<FeriasCliente>();
            feriasSalvasBase = feriasSalvasBase?.Where(x => x.DataCompetencia >= new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1))?.ToList();
            CalculoDescontoFerias(feriasSalvasBase, listaFeriasRemovidas);
        }

        private void CalculoDescontoFerias(List<FeriasCliente> listaFerias, List<FeriasCliente> listaFeriasRemovidas)
        {
            if ((!listaFerias.Any() || listaFerias == null)
                && (!listaFeriasRemovidas.Any() || listaFeriasRemovidas == null))
                return;

            var idCliente = listaFerias?.FirstOrDefault()?.Cliente?.Id ?? listaFeriasRemovidas?.FirstOrDefault()?.Cliente?.Id ?? 0;
            var listaContratosPorCliente = _contratoMensalistaServico.BuscarPorCliente(idCliente);
            var listaContratosPorClienteAtivos = listaContratosPorCliente?.Where(x => x.Ativo)?.ToList();
            var contaCorrente = _contaCorrenteClienteServico.BuscarPor(x => x.Cliente.Id == idCliente)?.LastOrDefault();

            var percentualBase = 0.5m; //50%

            foreach (var itemFerias in listaFerias)
            {
                foreach (var item in itemFerias.ListaFeriasClienteDetalhe)
                {
                    var diasCorridos = Convert.ToDecimal((item.DataFim.AddDays(1).Date - item.DataInicio.Date).TotalDays) > 30 
                                        ? 30 : Convert.ToDecimal((item.DataFim.AddDays(1).Date - item.DataInicio.Date).TotalDays);

                    if (item.DataFim.AddDays(1).Date.Day == DateTime.DaysInMonth(item.DataFim.AddDays(1).Date.Year, item.DataFim.AddDays(1).Date.Month))
                    {
                        var lastDayMonth = DateTime.DaysInMonth(item.DataFim.AddDays(1).Date.Year, item.DataFim.AddDays(1).Date.Month);
                        if (item.DataFim.AddDays(1).Date.Month == 2)
                        {
                            diasCorridos = 30 - lastDayMonth + diasCorridos;
                        }
                        else
                        {
                            diasCorridos = lastDayMonth == 31 ? diasCorridos - 1 : diasCorridos;
                        }
                    }

                    item.ValorFeriasCalculadaAnterior = item.ValorFeriasCalculada;

                    var valorTotalContrato = 0m;
                    if (itemFerias.InutilizarTodasVagas)
                    {
                        valorTotalContrato = listaContratosPorClienteAtivos.Sum(x => x.Valor * x.NumeroVagas);
                    }
                    else
                    {
                        var contrato = listaContratosPorClienteAtivos.FirstOrDefault(x => x.Id == itemFerias.ContratoMensalista.Id);
                        valorTotalContrato = contrato.Valor * itemFerias.TotalVagas;
                    }

                    //Recebe o Valor Total de Contrato(s) x Vaga(s), aplicando o percentual e dividindo pelo total de dias médio (30 dias).
                    var valorContratoDescontoPercentualDia = percentualBase * valorTotalContrato / 30;
                    item.ValorFeriasCalculada = decimal.Round(valorContratoDescontoPercentualDia * diasCorridos, 2, MidpointRounding.AwayFromZero);
                }
                itemFerias.ValorFeriasCalculadaAnterior = itemFerias.ListaFeriasClienteDetalhe.Sum(x => x.ValorFeriasCalculadaAnterior);
                itemFerias.ValorFeriasCalculada = itemFerias.ListaFeriasClienteDetalhe.Sum(x => x.ValorFeriasCalculada);
            }

            var listaFeriasContaCorrente = new List<FeriasCliente>();
            listaFeriasContaCorrente.AddRange(listaFerias);
            listaFeriasContaCorrente.AddRange(listaFeriasRemovidas);

            AtualizarContaCorrente(listaFeriasContaCorrente, contaCorrente);

            foreach (var item in listaFerias)
            {
                _feriasClienteServico.Salvar(item);
            }
        }

        private void AtualizarContaCorrente(List<FeriasCliente> ferias, ContaCorrenteCliente contaCorrente)
        {
            var idCliente = ferias?.FirstOrDefault()?.Cliente?.Id ?? 0;
            var listaPeriodosCompetencia = ferias?.SelectMany(x => x.ListaDataCompetenciaPeriodo)?.Distinct()?.ToList();

            foreach (var competencia in listaPeriodosCompetencia)
            {
                var valorFeriasCalculadasAnterior = ferias.SelectMany(x => x.ListaFeriasClienteDetalhe)
                                                            .Where(x => x.DataCompetencia == competencia)
                                                            .Sum(x => x.ValorFeriasCalculadaAnterior);
                var valorFeriasCalculadasAtual = ferias.SelectMany(x => x.ListaFeriasClienteDetalhe)
                                                            .Where(x => x.DataCompetencia == competencia)
                                                            .Sum(x => x.ValorFeriasCalculada);

                var itemCCDetalhe = contaCorrente?.ContaCorrenteClienteDetalhes?
                                                        .FirstOrDefault(x => x.TipoOperacaoContaCorrente == Entidade.Uteis.TipoOperacaoContaCorrente.Decrescimo
                                                                            && x.DataCompetencia == competencia);
                if (itemCCDetalhe != null && itemCCDetalhe.DataInsercao > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                {
                    itemCCDetalhe.Valor = ferias.SelectMany(x => x.ListaFeriasClienteDetalhe)
                                                .Where(x => x.DataCompetencia == competencia)
                                                .Sum(x => x.ValorFeriasCalculadaAnterior) > 0
                                                    ? itemCCDetalhe.Valor - valorFeriasCalculadasAnterior + valorFeriasCalculadasAtual
                                                    : itemCCDetalhe.Valor + valorFeriasCalculadasAtual;
                    _contaCorrenteClienteServico.Salvar(contaCorrente);
                }
                else if (contaCorrente != null && contaCorrente.Id > 0)
                {
                    contaCorrente.ContaCorrenteClienteDetalhes.Add(new ContaCorrenteClienteDetalhe
                    {
                        DataInsercao = DateTime.Now,
                        DataCompetencia = competencia,
                        TipoOperacaoContaCorrente = Entidade.Uteis.TipoOperacaoContaCorrente.Decrescimo,
                        Valor = valorFeriasCalculadasAtual
                    });
                    _contaCorrenteClienteServico.Salvar(contaCorrente);
                }
                else
                {
                    var cliente = _clienteServico.BuscarPorId(idCliente);
                    var contaCorrenteNova = new ContaCorrenteCliente
                    {
                        Cliente = cliente,
                        DataInsercao = DateTime.Now,
                        ContaCorrenteClienteDetalhes = new List<ContaCorrenteClienteDetalhe>
                        {
                            new ContaCorrenteClienteDetalhe
                            {
                                DataInsercao = DateTime.Now,
                                DataCompetencia = competencia,
                                TipoOperacaoContaCorrente = Entidade.Uteis.TipoOperacaoContaCorrente.Decrescimo,
                                Valor = valorFeriasCalculadasAtual
                            }
                        }
                    };
                    _contaCorrenteClienteServico.Salvar(contaCorrenteNova);
                    cliente.ContaCorrenteCliente = contaCorrente;
                    _clienteServico.Salvar(cliente);
                    contaCorrente = _clienteServico.BuscarPorId(idCliente)?.ContaCorrenteCliente;
                }

            }
        }
    }
}