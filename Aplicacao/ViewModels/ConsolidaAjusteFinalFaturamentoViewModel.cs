using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class ConsolidaAjusteFinalFaturamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public decimal DespesaFinal { get; set; }
        public decimal FaturamentoFinal { get; set; }
        public decimal Diferenca { get; set; }
        public string AjusteFinalFaturamento { get; set; }
    }
}
