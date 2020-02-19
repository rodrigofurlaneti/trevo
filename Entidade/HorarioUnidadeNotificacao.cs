using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class HorarioUnidadeNotificacao : BaseEntity
    {
        public virtual HorarioUnidade HorarioUnidade { get; set; }
        public virtual Notificacao Notificacao { get; set; }
    }
}
