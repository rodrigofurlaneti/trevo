using Entidade.Base;

namespace Entidade
{
    public class LancamentoCobrancaContratoMensalista : BaseEntity, IAudit
    {
        public virtual ContratoMensalista ContratoMensalista { get; set; }
        public virtual LancamentoCobranca LancamentoCobranca { get; set; }
    }
}