using Entidade.Base;

namespace Entidade
{
    public class ConsolidaAjusteFaturamento : BaseEntity
    {
        public virtual Unidade Unidade { get; set; }
        public virtual int Mes { get; set; }
        public virtual int Ano { get; set; }
        public virtual ConsolidaDespesa ConsolidaDespesa { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual ConsolidaFaturamento ConsolidaFaturamento { get; set; }
        public virtual ConsolidaAjusteFinalFaturamento ConsolidaAjusteFinalFaturamento { get; set; }
    }
}
