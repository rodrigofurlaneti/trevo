using Entidade.Base;

namespace Entidade
{
    public class ConsolidaDespesa : BaseEntity
    {
        public virtual decimal DespesaTotal { get; set; }
        public virtual decimal DespesaFixa { get; set; }
        public virtual decimal DespesaEscolhida { get; set; }
        public virtual decimal DespesaEscolhidaFixa { get; set; }
        public virtual decimal DespesaValorFinal { get; set; }

    }
}
