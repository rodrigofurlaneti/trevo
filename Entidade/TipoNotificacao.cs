using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class TipoNotificacao : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual Entidades Entidade { get; set; }
    }
}
