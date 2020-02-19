using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class NotificacaoAvisoPedidoSelo: BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual PedidoSelo PedidoSelo { get; set; }
    }
}