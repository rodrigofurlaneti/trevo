using Aplicacao.Base;
using Aplicacao.ViewModels;
using BoletoNet;
using Core.Exceptions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Extensions;
using System.Transactions;

namespace Aplicacao
{
    public interface ILancamentoCobrancaAplicacao : IBaseAplicacao<LancamentoCobranca>
    {
        IList<LancamentoCobranca> ListarLancamentosCobranca(BaixaManualViewModel filtro);
        IList<LancamentoCobranca> ListarLancamentosCobranca(GeracaoCNABLancamentoCobrancaViewModel filtro);
        void EfetuarPagamentoTotal(List<BaixaManualViewModel> dados, Usuario usuario);
        void EfetuarPagamentoParcial(List<BaixaManualViewModel> dados, Usuario usuario);

        GeracaoCNABLancamentoCobrancaViewModel GerarBoletosBancariosHtml(List<LancamentoCobranca> lancamentos, DateTime? dtVencimento, TipoValor tipoValorJuros, decimal? juros, TipoValor tipoValorMulta, decimal? multa, TipoOcorrenciaCNAB tipoOcorrenciaCNAB);
        BoletoNet.Boleto ImprimirBoletoBancario(LancamentoCobranca cobranca, ContaFinanceira contaFinanceira, DateTime? dtVencimento, TipoValor tipoValorJuros, decimal? juros, TipoValor tipoValorMulta, decimal? multa, List<ParametroBoletoBancario> listaParametroBoletoBancario, TipoOcorrenciaCNAB tipoOcorrenciaCNAB);
        IList<LancamentoCobranca> BuscarLancamentosPorCliente(int idCliente);
        IList<LancamentoCobranca> BuscarLancamentosCobranca(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, int numeroContrato, List<int> listaIds = null);
        IList<LancamentoCobranca> BuscarLancamentosCobrancaLinq(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, List<int> listaIds = null);

        IList<DadosLancamentosVO> BuscarLancamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
        IList<DadosLancamentosVO> BuscarLancamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
        IList<DadosPagamentoVO> BuscarPagamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
        IList<DadosPagamentoVO> BuscarPagamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade, bool acrescentarCancelados = false);
        IList<DadosPagamentoVO> BuscarPagamentosEfetuadosConferenciaRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
    }

    public class LancamentoCobrancaAplicacao : BaseAplicacao<LancamentoCobranca, ILancamentoCobrancaServico>, ILancamentoCobrancaAplicacao
    {
        private readonly ILancamentoCobrancaServico _lancamentoCobrancaServico;
        private readonly IContaFinanceiraServico _contaFinanceiraServico;
        private readonly IPagamentoAplicacao _pagamentoAplicacao;
        private readonly IEmpresaAplicacao _empresaAplicacao;
        private readonly IParametroBoletoBancarioServico _parametroBoletoBancarioServico;

        public LancamentoCobrancaAplicacao(
            ILancamentoCobrancaServico lancamentoCobrancaServico,
            IContaFinanceiraServico contaFinanceiraServico,
            IPagamentoAplicacao pagamentoAplicacao,
            IEmpresaAplicacao empresaAplicacao,
            IParametroBoletoBancarioServico parametroBoletoBancarioServico
        )
        {
            _lancamentoCobrancaServico = lancamentoCobrancaServico;
            _contaFinanceiraServico = contaFinanceiraServico;
            _pagamentoAplicacao = pagamentoAplicacao;
            _empresaAplicacao = empresaAplicacao;
            _parametroBoletoBancarioServico = parametroBoletoBancarioServico;
        }

        public IList<LancamentoCobranca> ListarLancamentosCobranca(BaixaManualViewModel filtro)
        {
            return _lancamentoCobrancaServico.ListarLancamentosCobranca(filtro.ContaFinanceira?.Id, filtro.TipoServico, filtro.DataVencimentoInicio, filtro.DataVencimentoFim, filtro.Cliente?.Id, filtro.Documento);
        }

        public IList<LancamentoCobranca> ListarLancamentosCobranca(GeracaoCNABLancamentoCobrancaViewModel filtro)
        {
            return _lancamentoCobrancaServico.ListarLancamentosCobranca(filtro.ContaFinanceira?.Id, filtro.TipoServico, 
                filtro.Unidade?.Id, filtro.StatusLancamentoCobranca, filtro.TipoFiltroGeracaoCNAB, 
                filtro.Supervisor.Id, filtro.Cliente.Id, filtro.DataDEFiltro, filtro.DataATEFiltro);
        }

        public void EfetuarPagamentoTotal(List<BaixaManualViewModel> dados, Usuario usuarioLogado)
        {
            foreach (var item in dados)
            {
                var lance = _lancamentoCobrancaServico.BuscarPorId(item.Id);
                if (lance.StatusLancamentoCobranca == StatusLancamentoCobranca.Pago)
                    continue;

                lance.DataBaixa = DateTime.Now;
                lance.StatusLancamentoCobranca = StatusLancamentoCobranca.Pago;

                if (lance.Recebimento == null)
                    lance.Recebimento = new Recebimento();
                if (lance.Recebimento.Pagamentos == null)
                    lance.Recebimento.Pagamentos = new List<Pagamento>();
                
                lance.Recebimento.Pagamentos.Add(new Pagamento
                {
                    ValorPago = !string.IsNullOrEmpty(item.ValorPago) ? Convert.ToDecimal(item.ValorPago) : 0,
                    TipoDescontoAcrescimo = item.TipoDescontoAcrescimo,
                    ValorDivergente = item.ValorDivergente,
                    Justificativa = item.Justificativa,
                    DataPagamento = item.DataPagamento,
                    NumeroRecibo = item.NumeroRecibo,
                    Recebimento = lance.Recebimento,
                    FormaPagamento = item.FormaPagamento,
                    Unidade = lance.Unidade,
                    ContaContabil = item.ContaContabil
                });
                
                _lancamentoCobrancaServico.Salvar(lance);

                _lancamentoCobrancaServico.GerarNotificacaoPagamento(lance, usuarioLogado);
            }
        }

        public void EfetuarPagamentoParcial(List<BaixaManualViewModel> dados, Usuario usuarioLogado)
        {
            foreach (var item in dados)
            {
                var lance = _lancamentoCobrancaServico.BuscarPorId(item.Id);
                if (lance.StatusLancamentoCobranca == StatusLancamentoCobranca.Pago)
                    continue;

                lance.DataVencimento = item.DataVencimento;
                lance.DataCompetencia = item.DataCompetencia.HasValue ? item.DataCompetencia : new DateTime(item.DataVencimento.Year, item.DataVencimento.Month, 1);

                if (lance.Recebimento == null)
                    lance.Recebimento = new Recebimento();
                if (lance.Recebimento.Pagamentos == null)
                    lance.Recebimento.Pagamentos = new List<Pagamento>();
                
                var pagamento = new Pagamento
                {
                    ValorPago = Convert.ToDecimal(item.ValorPago),
                    TipoDescontoAcrescimo = item.TipoDescontoAcrescimo,
                    ValorDivergente = item.ValorDivergente,
                    Justificativa = item.Justificativa,
                    DataPagamento = item.DataPagamento,
                    NumeroRecibo = item.NumeroRecibo,
                    Recebimento = lance.Recebimento,
                    FormaPagamento = item.FormaPagamento,
                    Unidade = lance.Unidade,
                    ContaContabil = item.ContaContabil

                };

                lance.Recebimento.Pagamentos.Add(pagamento);

                if (lance.ValorAReceber > 0)
                {
                    lance.StatusLancamentoCobranca = StatusLancamentoCobranca.EmAberto;
                }
                //lance.ValorContrato = lance.ValorContrato - Convert.ToDecimal(lance.ValorRecebido) <= 0 ? 0 : (lance.ValorContrato - Convert.ToDecimal(lance.ValorRecebido));

                if (lance.ValorAReceber <= 0)
                {
                    lance.StatusLancamentoCobranca = StatusLancamentoCobranca.Pago;
                    lance.DataBaixa = pagamento.DataPagamento;
                }

                _lancamentoCobrancaServico.Salvar(lance);

                if (lance.StatusLancamentoCobranca == StatusLancamentoCobranca.Pago)
                    _lancamentoCobrancaServico.GerarNotificacaoPagamento(lance, usuarioLogado);
            }
        }
        
        public GeracaoCNABLancamentoCobrancaViewModel GerarBoletosBancariosHtml(List<LancamentoCobranca> lancamentos, DateTime? dtVencimento, TipoValor tipoValorJuros, decimal? juros, TipoValor tipoValorMulta, decimal? multa, TipoOcorrenciaCNAB tipoOcorrenciaCNAB)
        {
            var geracaoCnab = new GeracaoCNABLancamentoCobrancaViewModel();

            using (TransactionScope scope = new TransactionScope())
            {
                var listaBoletoBancario = new List<BoletoBancario>();
                var listaBoletos = new Boletos();

                var contaFinanceiraLancamento = lancamentos?.FirstOrDefault()?.ContaFinanceira ?? new ContaFinanceira();

                var contaFinanceira = _contaFinanceiraServico.BuscarPorId(contaFinanceiraLancamento.Id);
                var listaParametroBoletoBancario = _parametroBoletoBancarioServico.Buscar()?.ToList() ?? new List<ParametroBoletoBancario>();

                foreach (var item in lancamentos)
                {
                    if (item.Cliente.Pessoa.Enderecos == null || !item.Cliente.Pessoa.Enderecos.Any())
                        throw new BusinessRuleException($"O cadastro de [{item.Cliente.Pessoa.Nome}], não possui Endereço. É necessário esta informação para prosseguir!");

                    var boleto = ImprimirBoletoBancario(item, contaFinanceira, dtVencimento, tipoValorJuros, juros, tipoValorMulta, multa, listaParametroBoletoBancario, tipoOcorrenciaCNAB);

                    var boletoBancario = new BoletoBancario()
                    {
                        CodigoBanco = Convert.ToInt16(contaFinanceira.Banco.CodigoBanco),
                        Boleto = boleto,
                        MostrarCodigoCarteira = true,
                        TextoAbaixoDoBoleto = $"UNIDADE: {item.Unidade.Nome} - {item.Unidade?.Endereco?.Logradouro}" +
                                                $"<br/>REF: {(item.DataCompetencia == null ? item.DataVencimento.ToString("yyyyMM") : item.DataCompetencia.Value.ToString("yyyyMM"))}" +
                                                $"{(item.TipoServico == TipoServico.Mensalista ? $"<br/>CONTRATO: {item.NumerosContratos}" : string.Empty)}"

                    };

                    boletoBancario.Boleto.Valida();
                    boletoBancario.MostrarComprovanteEntrega = false;
                    boletoBancario.FormatoCarne = true;
                    boletoBancario.GeraImagemCodigoBarras(boleto);

                    listaBoletos.Add(boleto);
                    listaBoletoBancario.Add(boletoBancario);

                    item.PossueCnab = true;
                    item.StatusLancamentoCobranca = StatusLancamentoCobranca.EmAberto;
                }
                
                var objCedente = new Cedente(
                    contaFinanceira.Cpf,
                    contaFinanceira.Descricao,
                    contaFinanceira.Agencia,
                    contaFinanceira.Conta,
                    contaFinanceira.DigitoConta
                )
                {
                    CodigoTransmissao = $"{contaFinanceira.Agencia}0{contaFinanceira.Convenio.Truncate(7).PadLeft(7, '0')}0{contaFinanceira.Conta.Truncate(7).PadLeft(7, '0')}",
                    Convenio = Convert.ToInt64(contaFinanceira.Convenio),
                    ContaBancaria = new ContaBancaria(contaFinanceira.Agencia, contaFinanceira.DigitoAgencia ?? string.Empty, contaFinanceira.Conta, contaFinanceira.DigitoConta),
                    Nome = contaFinanceira.Empresa?.RazaoSocial
                };


                var aqvRemessa = new ArquivoRemessa(BoletoNet.TipoArquivo.CNAB400);
                var banco = new BoletoNet.Banco(Convert.ToInt32(contaFinanceira.Banco.CodigoBanco));

                using (var mem = new MemoryStream())
                {
                    if (listaBoletos.Any())
                        aqvRemessa.GerarArquivoRemessa(contaFinanceira.Convenio, banco, objCedente, listaBoletos, mem, Convert.ToInt32(contaFinanceira.CodigoTransmissao));
                    
                    var pdfBytes = new BoletoBancario().MontaBytesListaBoletosPDF(listaBoletoBancario, "Boletos Gerados", null, "Boletos Gerados");
                    
                    geracaoCnab = new GeracaoCNABLancamentoCobrancaViewModel
                    {
                        BoletosHtml = listaBoletoBancario?.Select(x => x.MontaHtmlEmbedded())?.ToList() ?? new List<string>(),
                        ArquivoRemessaMemoryStream = mem,
                        DadosPDF = pdfBytes
                    };
                }
            }

            _lancamentoCobrancaServico.UpdateDetalhesCNAB(lancamentos);

            return geracaoCnab;
        }

        public BoletoNet.Boleto ImprimirBoletoBancario(LancamentoCobranca cobranca, ContaFinanceira contaFinanceira, DateTime? dtVencimento, TipoValor tipoValorJuros, decimal? juros, TipoValor tipoValorMulta, decimal? multa, List<ParametroBoletoBancario> listaParametroBoletoBancario, TipoOcorrenciaCNAB tipoOcorrenciaCNAB)
        {
            return _lancamentoCobrancaServico.ImprimirBoletoBancario(cobranca, contaFinanceira, dtVencimento, tipoValorJuros, juros, tipoValorMulta, multa, listaParametroBoletoBancario, tipoOcorrenciaCNAB);
        }

        public IList<LancamentoCobranca> BuscarLancamentosPorCliente(int idCliente)
        {
            return _lancamentoCobrancaServico.BuscarLancamentosPorCliente(idCliente);
        }

        public IList<LancamentoCobranca> BuscarLancamentosCobranca(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, int numeroContrato, List<int> listaIds = null)
        {
            return _lancamentoCobrancaServico.BuscarLancamentosCobranca(status, unidade, cliente, dataVencimento, numeroContrato, listaIds);
        }

        public IList<DadosLancamentosVO> BuscarLancamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            return _lancamentoCobrancaServico.BuscarLancamentosPagosRelatorio(dataInicio, dataFim, tipoServico, unidade);
        }
        public IList<DadosLancamentosVO> BuscarLancamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            return _lancamentoCobrancaServico.BuscarLancamentosEmAbertoRelatorio(dataInicio, dataFim, tipoServico, unidade);
        }

        public IList<DadosPagamentoVO> BuscarPagamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            return _lancamentoCobrancaServico.BuscarPagamentosPagosRelatorio(dataInicio, dataFim, tipoServico, unidade);
        }
        public IList<DadosPagamentoVO> BuscarPagamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade, bool acrescentarCancelados = false)
        {
            return _lancamentoCobrancaServico.BuscarPagamentosEmAbertoRelatorio(dataInicio, dataFim, tipoServico, unidade, acrescentarCancelados);
        }

        public IList<LancamentoCobranca> BuscarLancamentosCobrancaLinq(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, List<int> listaIds = null)
        {
            return _lancamentoCobrancaServico.BuscarLancamentosCobrancaLinq(status, unidade, cliente, dataVencimento, listaIds);
        }

        public IList<DadosPagamentoVO> BuscarPagamentosEfetuadosConferenciaRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            return _lancamentoCobrancaServico.BuscarPagamentosEfetuadosConferenciaRelatorio(dataInicio, dataFim, tipoServico, unidade);
        }
    }
}