using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class LancamentoCobrancaNotificacao
    {
        public virtual LancamentoCobranca LancamentoCobranca { get; set; }
        public virtual Notificacao Notificacao { get; set; }
    }
}