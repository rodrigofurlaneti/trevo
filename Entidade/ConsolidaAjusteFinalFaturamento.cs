using Entidade.Base;

namespace Entidade
{
    public class ConsolidaAjusteFinalFaturamento : BaseEntity
    {
        public virtual decimal DespesaFinal { get; set; }
        public virtual decimal FaturamentoFinal { get; set; }
        public virtual decimal Diferenca { get; set; }
        public virtual decimal AjusteFinalFaturamento { get; set; }
    }
}
