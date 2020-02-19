using Entidade.Base;

namespace Entidade
{
    public class TabelaPrecoAvulsoNotificacao : BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual TabelaPrecoAvulso TabelaPrecoAvulso { get; set; }
    }
}