using Aplicacao.Base;
using BoletoNet;
using Core.Exceptions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Linq;

namespace Aplicacao
{
    public interface ILancamentoCobrancaPedidoSeloAplicacao : IBaseAplicacao<LancamentoCobrancaPedidoSelo>
    {
        string GerarBoletoBancarioHtml(LancamentoCobranca lancamento);
        LancamentoCobrancaPedidoSelo RetornaUltimoLancamentoCobrancaPorPedidoSelo(int idPedido);
        StatusLancamentoCobranca RetornaStatusPorPedidoSelo(int idPedidoSelo);
    }

    public class LancamentoCobrancaPedidoSeloAplicacao : BaseAplicacao<LancamentoCobrancaPedidoSelo, ILancamentoCobrancaPedidoSeloServico>, ILancamentoCobrancaPedidoSeloAplicacao
    {
        private readonly ILancamentoCobrancaPedidoSeloServico _lancamentoCobrancaPedidoSeloServico;
        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;
        private readonly ILancamentoCobrancaServico _lancamentoCobrancaServico;
        private readonly IContaFinanceiraServico _contaFinanceiraServico;
        private readonly IPagamentoAplicacao _pagamentoAplicacao;

        public LancamentoCobrancaPedidoSeloAplicacao(
            ILancamentoCobrancaPedidoSeloServico lancamentoCobrancaPedidoSeloServico,
            ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao,
            ILancamentoCobrancaServico lancamentoCobrancaServico,
            IContaFinanceiraServico contaFinanceiraServico,
            IPagamentoAplicacao pagamentoAplicacao
        )
        {
            _lancamentoCobrancaPedidoSeloServico = lancamentoCobrancaPedidoSeloServico;
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            _lancamentoCobrancaServico = lancamentoCobrancaServico;
            _contaFinanceiraServico = contaFinanceiraServico;
            _pagamentoAplicacao = pagamentoAplicacao;
        }

        public string GerarBoletoBancarioHtml(LancamentoCobranca lancamento)
        {
            return _lancamentoCobrancaPedidoSeloServico.GerarBoletoBancarioHtml(lancamento);
        }

        public StatusLancamentoCobranca RetornaStatusPorPedidoSelo(int idPedidoSelo)
        {
            return _lancamentoCobrancaPedidoSeloServico.RetornaStatusPorPedidoSelo(idPedidoSelo);
        }

        public LancamentoCobrancaPedidoSelo RetornaUltimoLancamentoCobrancaPorPedidoSelo(int idPedido)
        {
            return _lancamentoCobrancaPedidoSeloServico.RetornaUltimoLancamentoCobrancaPorPedidoSelo(idPedido);
        }
    }
}