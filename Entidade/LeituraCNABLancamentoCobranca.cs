using Entidade.Base;

namespace Entidade
{
    public class LeituraCNABLancamentoCobranca : BaseEntity, IAudit
    {
        public virtual LeituraCNAB LeituraCNAB { get; set; }

        public virtual LancamentoCobranca LancamentoCobranca { get; set; }
    }
}
