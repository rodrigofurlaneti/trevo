using Entidade.Base;

namespace Entidade
{
    public class ChequeEmitidoContaPagar : BaseEntity
    {
        public virtual ContasAPagar ContaPagar { get; set; }
        public virtual ChequeEmitido ChequeEmitido { get; set; }
    }
}