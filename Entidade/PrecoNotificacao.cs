using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PrecoNotificacao : BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual Preco Preco { get; set; }
    }
}
