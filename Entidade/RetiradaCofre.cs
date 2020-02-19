using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Entidade
{
    public class RetiradaCofre : BaseEntity
    {
        public virtual ContasAPagar ContasAPagar { get; set; }
        public virtual StatusRetiradaCofre StatusRetiradaCofre { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual string Observacoes { get; set; }

        public virtual IList<RetiradaCofreNotificacao> RetiradaCofreNotificacoes { get; set; }
    }
}
