using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPedidoLocacaoLancamentoCobrancaServico : IBaseServico<PedidoLocacaoLancamentoCobranca>
    {
    }

    public class PedidoLocacaoLancamentoCobrancaServico : BaseServico<PedidoLocacaoLancamentoCobranca, IPedidoLocacaoLancamentoCobrancaRepositorio>, IPedidoLocacaoLancamentoCobrancaServico
    {
        private readonly IPedidoLocacaoLancamentoCobrancaRepositorio _pedidoLocacaoLancamentoCobrancaRepositorio;

        public PedidoLocacaoLancamentoCobrancaServico(IPedidoLocacaoLancamentoCobrancaRepositorio pedidoLocacaoLancamentoCobrancaRepositorio)
        {
            _pedidoLocacaoLancamentoCobrancaRepositorio = pedidoLocacaoLancamentoCobrancaRepositorio;
        }
    }
}