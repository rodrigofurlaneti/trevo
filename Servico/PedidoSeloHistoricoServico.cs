using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPedidoSeloHistoricoServico : IBaseServico<PedidoSeloHistorico>
    {
    }

    public class PedidoSeloHistoricoServico : BaseServico<PedidoSeloHistorico, IPedidoSeloHistoricoRepositorio>, IPedidoSeloHistoricoServico
    {
    }
}