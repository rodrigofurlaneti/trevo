using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class NegociacaoSeloDescontoNotificacao: BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual PedidoSelo PedidoSelo { get; set; }
    }
}