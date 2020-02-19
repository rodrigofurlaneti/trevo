using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class TabelaPrecoAvulsoPeriodo : BaseEntity
    {
        public virtual TipoPeriodo Periodo { get; set; }
        public virtual TabelaPrecoAvulso TabelaPrecoAvulso { get; set; }
    }
}