using Entidade.Base;

namespace Entidade
{
    public class TipoPagamento : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string Descricao { get; set; }
    }
}
