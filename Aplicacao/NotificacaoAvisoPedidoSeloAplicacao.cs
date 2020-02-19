using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface INotificacaoAvisoPedidoSeloAplicacao : IBaseAplicacao<NotificacaoAvisoPedidoSelo>
    {
    }

    public class NotificacaoAvisoPedidoSeloAplicacao : BaseAplicacao<NotificacaoAvisoPedidoSelo, INotificacaoAvisoPedidoSeloServico>, INotificacaoAvisoPedidoSeloAplicacao
    {
        public NotificacaoAvisoPedidoSeloAplicacao()
        {
        }
    }
}
