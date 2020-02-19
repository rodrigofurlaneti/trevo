using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class BoletagemAcaoViewModel
    {
        public TipoLote TipoLote { get; set; }
        public string Plano { get; set; }
        public OpcoesAgrupaContratos AgrupaContratos { get; set; }
        public DateTime DataVencimentoLote { get; set; }
        public decimal? ValorMinimo { get; set; }
        public decimal? ValorMinimoParcela { get; set; }
        public int ParametroCalculo { get; set; }
    }
}
