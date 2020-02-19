using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class ParametroEquipeNotificacao : BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual ParametroEquipe ParametroEquipe { get; set; }
    }
}
