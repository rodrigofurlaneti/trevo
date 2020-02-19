using Entidade.Base;

namespace Entidade
{
    public class ContratoUnidadeContasAPagar : BaseEntity
    {
        public virtual ContasAPagar ContaAPagar { get; set; }
        public virtual ContratoUnidade ContratoUnidade { get; set; }
    }
}