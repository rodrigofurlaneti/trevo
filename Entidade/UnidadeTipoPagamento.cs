using Entidade.Base;

namespace Entidade
{
    public class Tipospagamentos : BaseEntity
    {
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual int Unidade { get; set; }
    }
}
