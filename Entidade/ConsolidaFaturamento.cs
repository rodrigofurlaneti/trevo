using Entidade.Base;

namespace Entidade
{
    public class ConsolidaFaturamento : BaseEntity
    {
        public virtual decimal FaturamentoMes { get; set; }
        public virtual decimal FaturamentoCartao { get; set; }
        public virtual decimal Diferenca { get; set; }
        public virtual decimal FaturamentoFinal { get; set; }
    }
}
