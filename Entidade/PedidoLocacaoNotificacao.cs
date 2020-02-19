using Core.Exceptions;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PedidoLocacaoNotificacao : BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual PedidoLocacao PedidoLocacao { get; set; }

        public virtual void Aprovar(Usuario usuario)
        {
            Notificacao.Aprovar(usuario);
            PedidoLocacao.Status = StatusSolicitacao.Aprovado;
        }

        public virtual void Reprovar(Usuario usuario)
        {
            Notificacao.Reprovar(usuario);
            PedidoLocacao.Status = StatusSolicitacao.Negado;
        }

        public virtual bool PossuiNotificacaoDeDesconto()
        {
            return Notificacao.TipoNotificacao.Entidade == Entidades.Desconto;
        }
    }
}