using Dominio.IRepositorio.Base;
using Entidade;
using Entidade.Uteis;

namespace Dominio.IRepositorio
{
    public interface ILancamentoCobrancaPedidoSeloRepositorio : IRepository<LancamentoCobrancaPedidoSelo>
    {
        StatusLancamentoCobranca RetornaStatusPorPedidoSelo(int idPedidoSelo);
        LancamentoCobrancaPedidoSelo RetornaUltimoLancamentoCobrancaPorPedidoSelo(int idPedido);
    }
}