using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IPedidoLocacaoLancamentoCobrancaAplicacao : IBaseAplicacao<PedidoLocacaoLancamentoCobranca>
    {
    }

    public class PedidoLocacaoLancamentoCobrancaAplicacao : BaseAplicacao<PedidoLocacaoLancamentoCobranca, IPedidoLocacaoLancamentoCobrancaServico>, IPedidoLocacaoLancamentoCobrancaAplicacao
    {
        private readonly IPedidoLocacaoLancamentoCobrancaServico _pedidoLocacaoLancamentoCobrancaServico;
        
        public PedidoLocacaoLancamentoCobrancaAplicacao(IPedidoLocacaoLancamentoCobrancaServico pedidoLocacaoLancamentoCobrancaServico)
        {
            _pedidoLocacaoLancamentoCobrancaServico = pedidoLocacaoLancamentoCobrancaServico;
        }
    }
}