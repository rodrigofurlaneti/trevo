using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class TabelaPrecoMensalistaNotificacao : BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual TabelaPrecoMensalista TabelaPrecoMensalista { get; set; }
    }
}
