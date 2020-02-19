using BoletoNet;
using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface ILeituraCNABServico : IBaseServico<LeituraCNAB>
    {
        List<LeituraCNABPreviaVO> SalvarLeituraCNAB(LeituraCNAB leitura, List<DetalheRetorno> ListaRetornoCNAB, int usuarioId, bool previa = false);
        List<LeituraCNABPreviaVO> ProcessarLeitura(LeituraCNAB leitura, List<DetalheRetorno> ListaRetornoCNAB, int usuarioId, bool previa, NotificacaoDesbloqueioReferencia notificacaoDesbloqueioReferencia = null);
    }

    public class LeituraCNABServico : BaseServico<LeituraCNAB, ILeituraCNABRepositorio>, ILeituraCNABServico
    {
        private readonly ILancamentoCobrancaServico _lancamentoCobrancaServico;
        private readonly IParametroBoletoBancarioServico _parametroBoletoBancarioServico;
        private readonly IClienteServico _clienteServico;
        private readonly ILeituraCNABRepositorio _leituraCNABRepositorio;
        private readonly ILancamentoCobrancaPedidoSeloRepositorio _lancamentoCobrancaPedidoSeloRepositorio;

        public LeituraCNABServico(ILeituraCNABRepositorio leituraCNABRepositorio
            , ILancamentoCobrancaServico lancamentoCobrancaServico
            , IParametroBoletoBancarioServico parametroBoletoBancarioServico
            , IClienteServico clienteServico
            , ILancamentoCobrancaPedidoSeloRepositorio lancamentoCobrancaPedidoSeloRepositorio)
        {
            _leituraCNABRepositorio = leituraCNABRepositorio;
            _lancamentoCobrancaServico = lancamentoCobrancaServico;
            _parametroBoletoBancarioServico = parametroBoletoBancarioServico;
            _clienteServico = clienteServico;
            _lancamentoCobrancaPedidoSeloRepositorio = lancamentoCobrancaPedidoSeloRepositorio;
        }

        private LancamentoCobranca CriarLancamentoDivergente(int nossoNumero)
        {
            return new LancamentoCobranca
            {
                NossoNumero = nossoNumero,
                StatusLancamentoCobranca = StatusLancamentoCobranca.Divergente
            };
        }

        private string InformarPagamento(LancamentoCobranca lancamentoCobranca, DetalheRetorno detalheRetorno, Usuario usuario, decimal valorDesconto, bool onlyPreview = false)
        {
            if (lancamentoCobranca.Recebimento == null)
                lancamentoCobranca.Recebimento = new Recebimento();

            if (lancamentoCobranca.Recebimento.Pagamentos == null || !lancamentoCobranca.Recebimento.Pagamentos.Any())
                lancamentoCobranca.Recebimento.Pagamentos = new List<Pagamento>();

            if (lancamentoCobranca.Recebimento.Pagamentos.All(x => x.ValorPago != detalheRetorno.ValorPago))
                lancamentoCobranca.Recebimento.Pagamentos.Add(new Pagamento
                {
                    DataInsercao = DateTime.Now,
                    DataPagamento = detalheRetorno.DataCredito,
                    NossoNumero = Convert.ToInt32(detalheRetorno.NossoNumero),
                    FormaPagamento = FormaPagamento.Boleto,
                    ValorPago = detalheRetorno.ValorPago,
                    TipoDescontoAcrescimo = detalheRetorno.ValorPago == lancamentoCobranca.ValorTotal 
                                                ? TipoDescontoAcrescimo.SemDescontoSemAcrescimo 
                                                : detalheRetorno.ValorPago > lancamentoCobranca.ValorTotal 
                                                    ? TipoDescontoAcrescimo.Acrescimo 
                                                    : TipoDescontoAcrescimo.Desconto,
                    ValorDivergente = detalheRetorno.ValorPago == lancamentoCobranca.ValorTotal 
                                        ? 0
                                        : detalheRetorno.ValorPago > lancamentoCobranca.ValorTotal 
                                            ? Math.Abs(detalheRetorno.ValorPago - lancamentoCobranca.ValorTotal)
                                            : valorDesconto,
                    Justificativa = "Desconto por Parâmetro de Boleto Bancário (Leitura CNAB)",
                    StatusPagamento = true,
                    Recebimento = lancamentoCobranca.Recebimento
                });

            if (detalheRetorno.ValorPago == lancamentoCobranca.ValorTotal)
            {
                var descricao = $"O valor pago pelo Lançamento de Cobrança com o Nosso Número " +
                    $"[{(lancamentoCobranca.NossoNumero == 0 ? lancamentoCobranca.Id : lancamentoCobranca.NossoNumero)}] foi pago sem desconto.";
                if (onlyPreview) return descricao;
            }
            else if (detalheRetorno.ValorPago > lancamentoCobranca.ValorTotal)
            {
                var descricao = $"O valor pago pelo Lançamento de Cobrança com o Nosso Número " +
                    $"[{(lancamentoCobranca.NossoNumero == 0 ? lancamentoCobranca.Id : lancamentoCobranca.NossoNumero)}] foi maior do que o valor da divida.";

                if (onlyPreview) return descricao;

                _lancamentoCobrancaServico.SalvarNotificacaoDeAviso(lancamentoCobranca, usuario, descricao);
                return descricao;
            }
            return string.Empty;
        }

        public List<LeituraCNABPreviaVO> SalvarLeituraCNAB(LeituraCNAB leitura, List<DetalheRetorno> ListaRetornoCNAB, int usuarioId, bool previa = false)
        {
            if (PrimeiroPor(x => x.NomeArquivo == leitura.NomeArquivo) != null)
                throw new BusinessRuleException($"A leitura desse arquivo já foi realizada!");

            return ProcessarLeitura(leitura, ListaRetornoCNAB, usuarioId, previa);
        }

        public List<LeituraCNABPreviaVO> ProcessarLeitura(LeituraCNAB leitura, List<DetalheRetorno> ListaRetornoCNAB, int usuarioId, bool previa, NotificacaoDesbloqueioReferencia notificacaoDesbloqueioReferencia = null)
        {
            var retornoLeitura = new List<LeituraCNABPreviaVO>();

            var listaParametroBoletoBancario = _parametroBoletoBancarioServico.Buscar()?.ToList() ?? new List<ParametroBoletoBancario>();

            var _bloqueioReferenciaServico = ServiceLocator.Current.GetInstance<IBloqueioReferenciaServico>();

            var listaReferenciasBloqueadas = _bloqueioReferenciaServico.BuscarPor(x => x.Ativo == true)?.ToList() ?? new List<BloqueioReferencia>();

            foreach (var detalheRetorno in ListaRetornoCNAB)
            {
                var tipoOcorrenciaRetorno = Entidade.Uteis.TipoOcorrenciaRetorno.EntradaConfirmada;
                var conversaoOcorrenciaRetorno = Enum.TryParse(detalheRetorno.CodigoOcorrencia.ToString(), out tipoOcorrenciaRetorno);
                var previaLeituraCnab = new LeituraCNABPreviaVO
                {
                    NossoNumero = detalheRetorno.NossoNumero,
                    Cliente = detalheRetorno.NomeSacado,
                    DataPagamento = detalheRetorno.DataCredito,
                    ValorPago = detalheRetorno.ValorPago,
                    MotivoCodigoOcorrencia = detalheRetorno.MotivoCodigoOcorrencia,
                    CodigoOcorrencia = conversaoOcorrenciaRetorno ? tipoOcorrenciaRetorno.ToDescription() : string.Empty,
                    CampoErrosDoRetornoCNAB = string.IsNullOrEmpty(detalheRetorno.Erros) ? string.Empty : detalheRetorno.Erros,
                    DataVencimento = detalheRetorno.DataVencimento
                };

                if (!string.IsNullOrEmpty(detalheRetorno.Erros))
                {
                    retornoLeitura.Add(previaLeituraCnab);
                    continue;
                }

                var nossoNumero = Convert.ToInt32(detalheRetorno.NossoNumero);
                var lancamentoCobranca = _lancamentoCobrancaServico.BuscarPorId(nossoNumero);
                var nomeSacado = detalheRetorno.NomeSacado;

                if (lancamentoCobranca == null || lancamentoCobranca.Id == 0)
                {
                    var registrosDivergentes = _lancamentoCobrancaServico.BuscarPor(x => x.NossoNumero == nossoNumero)?.ToList();
                    if (registrosDivergentes.Any())
                    {
                        previaLeituraCnab.StatusCobranca = StatusLancamentoCobranca.Divergente.ToDescription();
                        previaLeituraCnab.Resultado = "Registro Divergente Já Cadastrado";
                        previaLeituraCnab.Observacao += $"A Cobrança no Arquivo de Retorno, Não foi Encontrada: [{nossoNumero}].";
                        retornoLeitura.Add(previaLeituraCnab);
                        continue;
                    }

                    lancamentoCobranca = CriarLancamentoDivergente(nossoNumero);
                    lancamentoCobranca.Cliente = _clienteServico.BuscarPor(x => x.NomeFantasia == nomeSacado.Trim())?.FirstOrDefault();
                    lancamentoCobranca.DataVencimento = detalheRetorno.DataVencimento;
                    lancamentoCobranca.DataCompetencia = lancamentoCobranca.DataCompetencia == null
                                                            ? new DateTime(lancamentoCobranca.DataVencimento.Year, lancamentoCobranca.DataVencimento.Month, 1)
                                                            : lancamentoCobranca.DataCompetencia;
                    lancamentoCobranca.DataGeracao = detalheRetorno.DataOcorrencia;
                    lancamentoCobranca.DataBaixa = detalheRetorno.DataLiquidacao;
                    lancamentoCobranca.PossueCnab = true;
                    lancamentoCobranca.ValorContrato = detalheRetorno.ValorTitulo;

                    if (!previa)
                    {
                        _lancamentoCobrancaServico.Salvar(lancamentoCobranca);
                        var descricao = $"Foi criado um Lançamento com o ID {lancamentoCobranca.Id} referente ao Nosso Número {nossoNumero}";
                        _lancamentoCobrancaServico.SalvarNotificacaoDeAviso(lancamentoCobranca, new Usuario { Id = usuarioId }, descricao);
                    }
                    previaLeituraCnab.Observacao += $"A Cobrança no Arquivo de Retorno, Não foi Encontrada: [{nossoNumero}]. ";
                }

                if (leitura.ListaLancamentos == null)
                    leitura.ListaLancamentos = new List<LeituraCNABLancamentoCobranca>();

                if (!leitura.ListaLancamentos.Any(x => x.LancamentoCobranca.Id == lancamentoCobranca.Id))
                    leitura.ListaLancamentos.Add(new LeituraCNABLancamentoCobranca
                    {
                        LeituraCNAB = leitura,
                        LancamentoCobranca = lancamentoCobranca
                    });

                previaLeituraCnab.CobrancaId = lancamentoCobranca.Id.ToString();

                if (lancamentoCobranca.StatusLancamentoCobranca == StatusLancamentoCobranca.Pago
                    && (lancamentoCobranca.Recebimento?.Pagamentos?.Any() ?? false))
                {
                    previaLeituraCnab.StatusCobranca = lancamentoCobranca.StatusLancamentoCobranca.ToDescription();
                    previaLeituraCnab.Resultado = "Já está Pago!";
                    retornoLeitura.Add(previaLeituraCnab);
                    continue;
                }

                if (detalheRetorno.CodigoOcorrencia == (int)Entidade.Uteis.TipoOcorrenciaRetorno.EntradaRejeitada)
                {
                    lancamentoCobranca.StatusLancamentoCobranca = StatusLancamentoCobranca.ErroCNAB;

                    previaLeituraCnab.StatusCobranca = lancamentoCobranca.StatusLancamentoCobranca.ToDescription();
                    previaLeituraCnab.Resultado = Entidade.Uteis.TipoOcorrenciaRetorno.EntradaRejeitada.ToDescription();
                    retornoLeitura.Add(previaLeituraCnab);
                    if (!previa)
                        _lancamentoCobrancaServico.Salvar(lancamentoCobranca);
                    continue;
                }

                if (detalheRetorno.CodigoOcorrencia == (int)Entidade.Uteis.TipoOcorrenciaRetorno.Baixa)
                {
                    lancamentoCobranca.StatusLancamentoCobranca = StatusLancamentoCobranca.Cancelado;
                    lancamentoCobranca.DataBaixa = detalheRetorno.DataOcorrencia;

                    previaLeituraCnab.StatusCobranca = lancamentoCobranca.StatusLancamentoCobranca.ToDescription();
                    previaLeituraCnab.DataBaixa = detalheRetorno.DataOcorrencia;
                    previaLeituraCnab.Resultado = "Erro: Baixa";
                    retornoLeitura.Add(previaLeituraCnab);
                    if (!previa)
                        _lancamentoCobrancaServico.Salvar(lancamentoCobranca);
                    continue;
                }

                if (detalheRetorno.CodigoOcorrencia == (int)Entidade.Uteis.TipoOcorrenciaRetorno.EntradaConfirmada)
                {
                    lancamentoCobranca.TipoOcorrenciaRetorno = (Entidade.Uteis.TipoOcorrenciaRetorno)detalheRetorno.CodigoOcorrencia;
                    lancamentoCobranca.NossoNumero = Convert.ToInt32(detalheRetorno.NossoNumeroComDV);

                    previaLeituraCnab.StatusCobranca = lancamentoCobranca.StatusLancamentoCobranca.ToDescription();
                    previaLeituraCnab.Resultado = $"Sucesso: [{Entidade.Uteis.TipoOcorrenciaRetorno.EntradaConfirmada.ToDescription()}]";
                    retornoLeitura.Add(previaLeituraCnab);
                    if (!previa)
                        _lancamentoCobrancaServico.Salvar(lancamentoCobranca);
                    continue;
                }

                if (detalheRetorno.ValorPago <= 0)
                {
                    previaLeituraCnab.StatusCobranca = lancamentoCobranca.StatusLancamentoCobranca.ToDescription();
                    previaLeituraCnab.Resultado = $"Erro: Valor Pago menor ou igual a Zero.";
                    retornoLeitura.Add(previaLeituraCnab);
                    continue;
                }

                var parametroBoleto =
                    listaParametroBoletoBancario?.FirstOrDefault(x => x.TipoServico == lancamentoCobranca.TipoServico && x.Unidade != null && x.Unidade.Id == lancamentoCobranca.Unidade.Id)
                    ??
                    listaParametroBoletoBancario?.FirstOrDefault(x => x.TipoServico == lancamentoCobranca.TipoServico);

                var observacao = string.Empty;
                var lancamentoBloqueado = false;
                if (!previa
                    && listaReferenciasBloqueadas.Any(x => x.DataMesAnoReferencia == lancamentoCobranca.DataCompetencia.Value))
                {
                    var retorno = notificacaoDesbloqueioReferencia != null
                                    ? new KeyValuePair<int, StatusDesbloqueioLiberacao>(notificacaoDesbloqueioReferencia.Id, notificacaoDesbloqueioReferencia.StatusDesbloqueioLiberacao)
                                    : _bloqueioReferenciaServico.ValidarLiberacao(0, lancamentoCobranca.Id, Entidades.LeituraCNAB, lancamentoCobranca.DataCompetencia.Value, new Usuario { Id = usuarioId }, leitura.NomeArquivo);
                    lancamentoBloqueado = retorno.Key > 0 && retorno.Value != StatusDesbloqueioLiberacao.Aprovado;
                }

                lancamentoCobranca.DataBaixa = detalheRetorno.DataCredito;
                var valorDesconto = decimal.Round((parametroBoleto?.ValorDesconto ?? 0) == 0 ? 0 : lancamentoCobranca.ValorTotal * (parametroBoleto?.ValorDesconto ?? 0), 2, MidpointRounding.AwayFromZero);
                observacao = InformarPagamento(lancamentoCobranca, detalheRetorno, new Usuario { Id = usuarioId }, valorDesconto, previa || lancamentoBloqueado);

                lancamentoCobranca.StatusLancamentoCobranca = lancamentoBloqueado
                    ? lancamentoCobranca.StatusLancamentoCobranca
                    : lancamentoCobranca.ValorAReceber - valorDesconto <= 0
                            ? StatusLancamentoCobranca.Pago
                            : StatusLancamentoCobranca.EmAberto;

                previaLeituraCnab.DataBaixa = lancamentoBloqueado ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : detalheRetorno.DataCredito;
                previaLeituraCnab.Observacao += lancamentoBloqueado ? "Bloqueio de Ref." : observacao;
                previaLeituraCnab.ValorDesconto = lancamentoBloqueado ? 0 : valorDesconto;
                previaLeituraCnab.StatusCobranca = lancamentoCobranca.StatusLancamentoCobranca.ToDescription();
                previaLeituraCnab.Resultado = lancamentoBloqueado ? "Bloqueio de Ref." : "Sucesso";
                retornoLeitura.Add(previaLeituraCnab);

                if (!previa && !lancamentoBloqueado)
                    _lancamentoCobrancaServico.Salvar(lancamentoCobranca);

                if (!previa && !lancamentoBloqueado && lancamentoCobranca.StatusLancamentoCobranca == StatusLancamentoCobranca.Pago)
                    _lancamentoCobrancaServico.GerarNotificacaoPagamento(lancamentoCobranca, new Usuario { Id = usuarioId });
            }

            if (!previa
                && leitura.ListaLancamentos != null && leitura.ListaLancamentos.Any())
                Salvar(leitura);

            return retornoLeitura;
        }
    }
}