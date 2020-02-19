using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class ConsolidaAjusteFaturamentoViewModel
    {
        public int Id { get; set; }
        public Mes Mes { get; set; }
        public int Ano { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public ConsolidaDespesaViewModel  ConsolidaDespesa { get; set; }
        public ConsolidaFaturamentoViewModel ConsolidaFaturamento { get; set; }
        public ConsolidaAjusteFinalFaturamentoViewModel ConsolidaAjusteFinalFaturamento { get; set; }
        public EmpresaViewModel Empresa { get; set; }
    }
}
