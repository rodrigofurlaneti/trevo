using System;
using Core.Attributes;

namespace Entidade.Base
{
    public class BaseEnitidadeNotificacao
    {
        public virtual Notificacao Notificacao { get; set; }

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