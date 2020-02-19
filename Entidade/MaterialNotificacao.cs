using Core.Exceptions;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class MaterialNotificacao : BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual Material Material { get; set; }

        public virtual void Aprovar(Usuario usuario)
        {
            Notificacao.Aprovar(usuario);
        }

        public virtual void Reprovar(Usuario usuario)
        {
            Notificacao.Reprovar(usuario);
        }
    }
}