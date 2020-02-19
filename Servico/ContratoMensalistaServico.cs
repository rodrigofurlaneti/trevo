using System.Collections.Generic;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;
using Core.Exceptions;
using System.Linq;
using System.Configuration;
using Entidade.Uteis;
using System.Web;

namespace Dominio
{
    public interface IContratoMensalistaServico : IBaseServico<ContratoMensalista>
    {
        void Salvar(ContratoMensalista contratoMensalista, bool validar);
        List<ContratoMensalista> BuscarPorIntervaloOrdenadoPeloNomeDoCliente(int registroInicial, int quantidadeRegistros);
        IList<ContratoMensalista> BuscarPorCliente(int idCliente);
        void GerarNotificacaoSePertoDeVencer();
    }

    public class ContratoMensalistaServico : BaseServico<ContratoMensalista, IContratoMensalistaRepositorio>, IContratoMensalistaServico
    {
        #region Private Properties
        private readonly IContratoMensalistaRepositorio _contratoMensalistaRepositorio;
        private readonly IContratoMensalistaVeiculoRepositorio _contratoMensalistaVeiculoRepositorio;
        private readonly ILancamentoCobrancaRepositorio _lancamentoCobrancaRepositorio;
        private readonly ILancamentoCobrancaContratoMensalistaRepositorio _lancamentoCobrancaContratoMensalistaRepositorio;
        private readonly IContaFinanceiraRepositorio _contaFinanceiraRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IUnidadeRepositorio _unidadeRepositorio;
        #endregion

        #region Constructor
        public ContratoMensalistaServico(IContratoMensalistaRepositorio contratoMensalistaRepositorio,
                                         IContratoMensalistaVeiculoRepositorio contratoMensalistaVeiculoRepositorio,
                                         ILancamentoCobrancaRepositorio lancamentoCobrancaRepositorio,
                                         ILancamentoCobrancaContratoMensalistaRepositorio lancamentoCobrancaContratoMensalistaRepositorio,
                                         IContaFinanceiraRepositorio contaFinanceiraRepositorio,
                                         INotificacaoRepositorio notificacaoRepositorio,
                                         IUnidadeRepositorio unidadeRepositorio)
        {
            _contratoMensalistaRepositorio = contratoMensalistaRepositorio;
            _contratoMensalistaVeiculoRepositorio = contratoMensalistaVeiculoRepositorio;
            _lancamentoCobrancaRepositorio = lancamentoCobrancaRepositorio;
            _lancamentoCobrancaContratoMensalistaRepositorio = lancamentoCobrancaContratoMensalistaRepositorio;
            _contaFinanceiraRepositorio = contaFinanceiraRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _unidadeRepositorio = unidadeRepositorio;
        }

        public List<ContratoMensalista> BuscarPorIntervaloOrdenadoPeloNomeDoCliente(int registroInicial, int quantidadeRegistros)
        {
            return _contratoMensalistaRepositorio.BuscarPorIntervaloOrdenadoPeloNomeDoCliente(registroInicial, quantidadeRegistros).ToList();
        }
        #endregion

        public void Salvar(ContratoMensalista contratoMensalista, bool validar = true)
        {
            var associarLancamento = false;
            var lancamentoParaAssociar = new LancamentoCobranca();
            var lancamentoParaAssociarExtra = new LancamentoCobranca();

            if (validar)
                ValidaContratoMensalista(contratoMensalista);

            ContaFinanceira contaFinanceira;

            if (contratoMensalista.Unidade != null && contratoMensalista.Unidade.Empresa != null)
                contaFinanceira = _contaFinanceiraRepositorio.FirstBy(x => x.Empresa.Id == contratoMensalista.Unidade.Empresa.Id);

            contaFinanceira = _contaFinanceiraRepositorio.FirstBy(x => x.ContaPadrao);

            if (contratoMensalista.Id == 0)
            {
                if (contratoMensalista.ContratoMensalistaNotificacoes == null)
                    contratoMensalista.ContratoMensalistaNotificacoes = new List<ContratoMensalistaNotificacao>();

                if (contratoMensalista.LancamentoCobrancas == null)
                    contratoMensalista.LancamentoCobrancas = new List<LancamentoCobranca>();

                var mensalidadeContrato = CalculoPorDiaVencimento(contratoMensalista);
                var valorCobrancaInicial = mensalidadeContrato.StatusPMS == StatusPMS.ValorLiquido ? mensalidadeContrato.ValorResultadoFinal : mensalidadeContrato.ValorDia;

                var valorContrato = decimal.Round(valorCobrancaInicial * contratoMensalista.NumeroVagas, 2, MidpointRounding.AwayFromZero);
                var valorContratoExtra = decimal.Round(mensalidadeContrato.ValorMensalidade * contratoMensalista.NumeroVagas, 2, MidpointRounding.AwayFromZero);

                var lancamentoCobranca = new LancamentoCobranca
                {
                    Cliente = contratoMensalista.Cliente,
                    DataGeracao = DateTime.Now,
                    DataVencimento = contratoMensalista.DataInicio,
                    DataCompetencia = new DateTime(contratoMensalista.DataInicio.Year, contratoMensalista.DataInicio.Month, 1),
                    TipoServico = TipoServico.Mensalista,
                    Unidade = contratoMensalista.Unidade,
                    ValorContrato = valorContrato, //contratoMensalista.Valor * contratoMensalista.NumeroVagas,
                    StatusLancamentoCobranca = StatusLancamentoCobranca.Novo,
                    ContaFinanceira = contaFinanceira
                };

                var valorPagoMensalidade = 0m;
                var valorPagoRestante = 0m;
                if (contratoMensalista.ValorPago > 0)
                {
                    var valorResultadoFinal = decimal.Round(mensalidadeContrato.ValorResultadoFinal * contratoMensalista.NumeroVagas, 2, MidpointRounding.AwayFromZero);
                    var valorAcrescimoOuDesconto = contratoMensalista.ValorPago > valorResultadoFinal 
                                                    ? TipoDescontoAcrescimo.Acrescimo
                                                    : contratoMensalista.ValorPago < valorResultadoFinal 
                                                        ? TipoDescontoAcrescimo.Desconto
                                                        : TipoDescontoAcrescimo.SemDescontoSemAcrescimo;
                    var valorDiferencaFinal = (contratoMensalista.ValorPago > valorResultadoFinal 
                                                ? contratoMensalista.ValorPago - valorResultadoFinal 
                                                : valorResultadoFinal - contratoMensalista.ValorPago).ToString("N2");

                    if (contratoMensalista.ValorPago != valorResultadoFinal
                        && valorAcrescimoOuDesconto != TipoDescontoAcrescimo.SemDescontoSemAcrescimo
                        && !contratoMensalista.PagamentoCadastro)
                        throw new MonthlyContractPaymentException($"O valor pago informado no campo, foi de [{contratoMensalista.ValorPago.ToString("N2")}], com isto tem um [{valorAcrescimoOuDesconto.GetDescription()}] de [{valorDiferencaFinal}], sendo o valor total de [{valorResultadoFinal.ToString("N2")}].<br/><br/>Deseja continuar mesmo assim?");

                    valorPagoMensalidade = mensalidadeContrato.StatusPMS == StatusPMS.ValorLiquido
                                            ? contratoMensalista.ValorPago
                                            : mensalidadeContrato.StatusPMS == StatusPMS.ProporcionalMesSeguinte
                                                ? contratoMensalista.ValorPago > valorContratoExtra
                                                    ? valorContratoExtra
                                                    : contratoMensalista.ValorPago
                                                : 0m;
                    valorPagoRestante = contratoMensalista.ValorPago - valorPagoMensalidade;

                    lancamentoCobranca.DataBaixa = contratoMensalista.DataInicio.Date;
                    lancamentoCobranca.StatusLancamentoCobranca = StatusLancamentoCobranca.Pago;

                    if (lancamentoCobranca.Recebimento == null)
                        lancamentoCobranca.Recebimento = new Recebimento();
                    if (lancamentoCobranca.Recebimento.Pagamentos == null)
                        lancamentoCobranca.Recebimento.Pagamentos = new List<Pagamento>();
                    lancamentoCobranca.Recebimento.Pagamentos.Add(new Pagamento
                    {
                        ValorPago = mensalidadeContrato.StatusPMS == StatusPMS.ValorLiquido ? valorPagoMensalidade : valorPagoRestante,
                        TipoDescontoAcrescimo = mensalidadeContrato.StatusPMS == StatusPMS.ValorLiquido
                                                    ? contratoMensalista.ValorPago > valorPagoMensalidade 
                                                        ? TipoDescontoAcrescimo.Acrescimo 
                                                        : contratoMensalista.ValorPago < valorPagoMensalidade
                                                            ? TipoDescontoAcrescimo.Desconto 
                                                            : TipoDescontoAcrescimo.SemDescontoSemAcrescimo
                                                    : valorPagoRestante > valorContrato
                                                        ? TipoDescontoAcrescimo.Acrescimo
                                                        : valorPagoRestante < valorContrato
                                                            ? TipoDescontoAcrescimo.Desconto
                                                            : TipoDescontoAcrescimo.SemDescontoSemAcrescimo,
                        ValorDivergente = Math.Abs(mensalidadeContrato.StatusPMS == StatusPMS.ValorLiquido
                                                    ? contratoMensalista.ValorPago - valorContrato
                                                    : contratoMensalista.ValorPago - valorPagoMensalidade - valorContrato),
                        DataPagamento = DateTime.Now,
                        NumeroRecibo = contratoMensalista.NumeroRecibo,
                        Recebimento = lancamentoCobranca.Recebimento,
                        FormaPagamento = FormaPagamento.Dinheiro,
                        Unidade = contratoMensalista.Unidade
                    });
                }

                _lancamentoCobrancaRepositorio.Save(lancamentoCobranca);
                contratoMensalista.LancamentoCobrancas.Add(lancamentoCobranca);
                associarLancamento = true;
                lancamentoParaAssociar = lancamentoCobranca;

                if (mensalidadeContrato.StatusPMS == StatusPMS.ProporcionalMesSeguinte)
                {
                    lancamentoParaAssociarExtra = new LancamentoCobranca
                    {
                        Cliente = contratoMensalista.Cliente,
                        DataGeracao = DateTime.Now,
                        DataVencimento = contratoMensalista.DataInicio,
                        DataCompetencia = new DateTime(contratoMensalista.DataInicio.AddMonths(1).Year, contratoMensalista.DataInicio.AddMonths(1).Month, 1),
                        TipoServico = TipoServico.Mensalista,
                        Unidade = contratoMensalista.Unidade,
                        ValorContrato = valorContratoExtra,
                        StatusLancamentoCobranca = StatusLancamentoCobranca.Novo,
                        ContaFinanceira = contaFinanceira
                    };

                    if (contratoMensalista.ValorPago > 0)
                    {
                        lancamentoParaAssociarExtra.DataBaixa = contratoMensalista.DataInicio.Date;
                        lancamentoParaAssociarExtra.StatusLancamentoCobranca = StatusLancamentoCobranca.Pago;

                        if (lancamentoParaAssociarExtra.Recebimento == null)
                            lancamentoParaAssociarExtra.Recebimento = new Recebimento();
                        if (lancamentoParaAssociarExtra.Recebimento.Pagamentos == null)
                            lancamentoParaAssociarExtra.Recebimento.Pagamentos = new List<Pagamento>();
                        lancamentoParaAssociarExtra.Recebimento.Pagamentos.Add(new Pagamento
                        {
                            ValorPago = valorPagoMensalidade,
                            TipoDescontoAcrescimo = contratoMensalista.ValorPago < valorPagoMensalidade
                                                                ? TipoDescontoAcrescimo.Desconto
                                                                : TipoDescontoAcrescimo.SemDescontoSemAcrescimo,
                            ValorDivergente = Math.Abs(contratoMensalista.ValorPago < valorPagoMensalidade ? contratoMensalista.ValorPago - valorPagoMensalidade : 0m),
                            DataPagamento = DateTime.Now,
                            NumeroRecibo = contratoMensalista.NumeroRecibo,
                            Recebimento = lancamentoParaAssociarExtra.Recebimento,
                            FormaPagamento = FormaPagamento.Dinheiro,
                            Unidade = contratoMensalista.Unidade
                        });
                    }

                    _lancamentoCobrancaRepositorio.Save(lancamentoParaAssociarExtra);
                    contratoMensalista.LancamentoCobrancas.Add(lancamentoParaAssociarExtra);
                }
                
                //if (contratoMensalista.LancamentoCobrancas.Count(x => x.ValorContrato == lancamentoCobranca.ValorContrato
                //                                                        && x.DataVencimento == lancamentoCobranca.DataVencimento) <= 0)
                //{
                //    _lancamentoCobrancaRepositorio.Save(lancamentoCobranca);
                //    contratoMensalista.LancamentoCobrancas.Add(lancamentoCobranca);
                //    associarLancamento = true;
                //    lancamentoParaAssociar = lancamentoCobranca;
                //}
            }
            else
            {
                var contratoMensalistaSalvo = _contratoMensalistaRepositorio.GetById(contratoMensalista.Id);
                contratoMensalista.ContratoMensalistaNotificacoes = contratoMensalistaSalvo.ContratoMensalistaNotificacoes;
            }

            base.Salvar(contratoMensalista);
            if (associarLancamento)
            {
                _lancamentoCobrancaContratoMensalistaRepositorio.Save(new LancamentoCobrancaContratoMensalista
                {
                    LancamentoCobranca = lancamentoParaAssociar,
                    ContratoMensalista = contratoMensalista
                });
                if (lancamentoParaAssociarExtra != null && lancamentoParaAssociarExtra.ValorContrato > 0)
                {
                    _lancamentoCobrancaContratoMensalistaRepositorio.Save(new LancamentoCobrancaContratoMensalista
                    {
                        LancamentoCobranca = lancamentoParaAssociarExtra,
                        ContratoMensalista = contratoMensalista
                    });
                }
            }
        }

        private PMSVO CalculoPorDiaVencimento(ContratoMensalista contratoMensalista)
        {
            var diaInicio = contratoMensalista.DataInicio.Day == 31 ? 30 : contratoMensalista.DataInicio.Day;

            var valorMensalidadeComDesconto = contratoMensalista.Valor - (contratoMensalista.Valor * 0.08m);
            //Valor por Dia é sobre a Mensalidade Sem Desconto!
            var valorPorDia = contratoMensalista.Valor / 30;

            var diaDeVencimento = _unidadeRepositorio.GetById(contratoMensalista.Unidade.Id)?.DiaVencimento ?? 1;
            var maxDiasMensalidadeLiquida = 2;
            var maxDiasProporcional = 11;

            var valoresPorDiaNaUnidade = new Dictionary<int, PMSVO>();

            for (int i = 1; i <= 30; i++)
            {
                var diaAnteriorLista = valoresPorDiaNaUnidade.Any() ? valoresPorDiaNaUnidade.LastOrDefault().Value.Dia : diaDeVencimento;
                var diaSeguinteLista = !valoresPorDiaNaUnidade.Any()
                                        ? diaDeVencimento
                                        : diaAnteriorLista == 30
                                            ? 1 : diaAnteriorLista + 1;

                var totalDiasMes = 30 - diaSeguinteLista + 1;
                
                //var valor = diaSeguinteLista >= diaDeVencimento && diaSeguinteLista <= (diaDeVencimento + maxDiasMensalidadeLiquida)
                //            ? valorMensalidadeComDesconto
                //            : diaSeguinteLista >= (diaDeVencimento + maxDiasMensalidadeLiquida)
                //                && diaSeguinteLista <= (diaDeVencimento + maxDiasMensalidadeLiquida + maxDiasProporcional)
                //                    ? diaDeVencimento > 1
                //                        ? (totalDiasMes + diaDeVencimento - 1) * valorPorDia
                //                        : totalDiasMes * valorPorDia
                //                    : diaSeguinteLista < diaDeVencimento
                //                        ? valorPorDia * (diaDeVencimento - diaSeguinteLista) + valorMensalidadeComDesconto
                //                        : diaDeVencimento > 1
                //                            ? (totalDiasMes + diaDeVencimento - 1) * valorPorDia + valorMensalidadeComDesconto
                //                            : totalDiasMes * valorPorDia + valorMensalidadeComDesconto;
                var valorMensa = 0m;
                var valorDia = 0m;
                var statusPms = StatusPMS.ValorLiquido;
                if (diaSeguinteLista >= diaDeVencimento && diaSeguinteLista <= (diaDeVencimento + maxDiasMensalidadeLiquida))
                {
                    valorMensa = valorMensalidadeComDesconto;
                    statusPms = StatusPMS.ValorLiquido;
                }
                else if (diaSeguinteLista >= (diaDeVencimento + maxDiasMensalidadeLiquida) && diaSeguinteLista <= (diaDeVencimento + maxDiasMensalidadeLiquida + maxDiasProporcional))
                {
                    valorDia = diaDeVencimento > 1
                                ? (totalDiasMes + diaDeVencimento - 1) * valorPorDia
                                : totalDiasMes * valorPorDia;
                    statusPms = StatusPMS.Proporcional;
                }
                else if (diaSeguinteLista < diaDeVencimento)
                {
                    valorMensa = valorMensalidadeComDesconto;
                    valorDia = valorPorDia * (diaDeVencimento - diaSeguinteLista);
                    statusPms = StatusPMS.ProporcionalMesSeguinte;
                }
                else if (diaDeVencimento > 1)
                {
                    valorMensa = valorMensalidadeComDesconto;
                    valorDia = (totalDiasMes + diaDeVencimento - 1) * valorPorDia;
                    statusPms = StatusPMS.ProporcionalMesSeguinte;
                }
                else
                {
                    valorMensa = valorMensalidadeComDesconto;
                    valorDia = totalDiasMes * valorPorDia;
                    statusPms = StatusPMS.ProporcionalMesSeguinte;
                };

                valoresPorDiaNaUnidade.Add(i, new PMSVO(diaSeguinteLista, decimal.Round(valorMensa, 2, MidpointRounding.AwayFromZero), decimal.Round(valorDia, 2, MidpointRounding.AwayFromZero), statusPms));
            }

            //var valorFinalMensalidade = valoresPorDiaNaUnidade.FirstOrDefault(x => x.Value.Dia == contratoMensalista.DataInicio.Day).Value.ValorResultadoFinal;

            //var resultado = new KeyValuePair<KeyValuePair<decimal, decimal>, bool>(valorFinalMensalidade, (diaDeVencimento + maxDiasMensalidadeLiquida + maxDiasProporcional) <= diaInicio);
            //return new KeyValuePair<KeyValuePair<decimal, decimal>, bool>(valoresPorDiaNaUnidade.FirstOrDefault(x => x.Value.Key == contratoMensalista.DataInicio.Day).Value.Value, diaDeVencimento <= diaInicio);

            return valoresPorDiaNaUnidade.FirstOrDefault(x => x.Value.Dia == contratoMensalista.DataInicio.Day).Value;
        }

        public void ValidaContratoMensalista(ContratoMensalista entity)
        {
            if (entity.Cliente == null)
                throw new BusinessRuleException("Informe o Cliente!");

            if (entity.TipoMensalista == null)
                throw new BusinessRuleException("Informe o Tipo de Mensalista!");

            if (entity.Unidade == null)
                throw new BusinessRuleException("Informe a Unidade!");
            
            if (entity.DataVencimento.Date <= DateTime.Now.Date && entity.Id == 0)
                throw new BusinessRuleException("Informe uma data de vencimento válida!");

            if (entity.Valor <= 0)
                throw new BusinessRuleException("Informe um valor válido!");

            //if (entity.Contrato == null)
            //    throw new BusinessRuleException("Informe um contrato válido!");

            if (entity.DataInicio == DateTime.MinValue)
                throw new BusinessRuleException("Informe uma data de início válida!");

            //if (entity.DataFim == DateTime.MinValue)
            //    throw new BusinessRuleException("Informe uma data final válida!");
        }

        public IList<ContratoMensalista> BuscarPorCliente(int idCliente)
        {
            return _contratoMensalistaRepositorio.BuscarPorCliente(idCliente);
        }

        public void GerarNotificacaoSePertoDeVencer()
        {
            var usuario = HttpContext.Current.User as dynamic;
            var dias = int.Parse(ConfigurationManager.AppSettings["CONTRATO_DIAS_VENCIMENTO"]);
            var date = DateTime.Now.AddDays(dias).Date;
            var contratos = _contratoMensalistaRepositorio.ListBy(x => x.DataVencimento == date);

            foreach (var item in contratos)
            {
                var descricao = $"O Contrato {item.NumeroContrato} vencerá em {item.DataVencimento.ToShortDateString()}.";

                if (!item.ContratoMensalistaNotificacoes.Any(x => x.Notificacao.Descricao == descricao))
                {
                    var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(item,
                                                                                            Entidades.ContratoMensalista,
                                                                                            new Usuario { Id = usuario.UsuarioId },
                                                                                            item.DataVencimento.AddDays(1).AddHours(23).AddMinutes(59).AddSeconds(59),
                                                                                            descricao,
                                                                                            string.Empty,
                                                                                            TipoAcaoNotificacao.Aviso);

                    var contratoMensalistaNotificacao = new ContratoMensalistaNotificacao
                    {
                        ContratoMensalista = item,
                        Notificacao = notificacao
                    };

                    item.ContratoMensalistaNotificacoes.Add(contratoMensalistaNotificacao);
                }
            }

            _contratoMensalistaRepositorio.Save(contratos);
        }
    }
}