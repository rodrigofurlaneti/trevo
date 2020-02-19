using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class NecessidadeCompraNotificacao : BaseEntity
    {
        public virtual Notificacao Notificacao { get; set; }
        public virtual NecessidadeCompra NecessidadeCompra { get; set; }

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