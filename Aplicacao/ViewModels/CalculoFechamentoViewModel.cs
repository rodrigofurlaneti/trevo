using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class CalculoFechamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public bool PrefeituraMaiorIgualCartao { get; set; }
        public bool ValorComplementarEmitido { get; set; }
        public bool PrefeituraComplementarMaiorIgualDespesa { get; set; }
        public decimal ValorNotaEmissao { get; set; }
        public ConsolidaAjusteFaturamentoViewModel AjusteFaturamento { get; set; }
    }
}
