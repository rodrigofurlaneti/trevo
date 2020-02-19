using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class ContaPagarNotificacao
    {
        public virtual ContasAPagar ContasAPagar { get; set; }
        public virtual Notificacao Notificacao { get; set; }
    }
}