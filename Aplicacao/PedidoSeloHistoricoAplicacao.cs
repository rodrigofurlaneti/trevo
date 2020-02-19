using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPedidoSeloHistoricoAplicacao : IBaseAplicacao<PedidoSeloHistorico>
    {
    }

    public class PedidoSeloHistoricoAplicacao : BaseAplicacao<PedidoSeloHistorico, IPedidoSeloHistoricoServico>, IPedidoSeloHistoricoAplicacao
    {
    }
}