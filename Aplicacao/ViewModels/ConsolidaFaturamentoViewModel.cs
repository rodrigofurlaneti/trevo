using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class ConsolidaFaturamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public decimal FaturamentoMes { get; set; }
        public decimal FaturamentoCartao { get; set; }
        public decimal Diferenca { get; set; }
        public decimal FaturamentoFinal { get; set; }
    }
}
