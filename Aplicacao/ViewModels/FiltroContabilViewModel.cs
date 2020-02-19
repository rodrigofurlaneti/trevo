using System;

namespace Aplicacao.ViewModels
{
    public class FiltroContabilViewModel
    {
        public int Credor { get; set; }
        public int Produto { get; set; }
        public int Carteira { get; set; }
        public string CPF { get; set; }
	    public bool EmNegociacao { get; set; }
	    public bool EmAberto { get; set; }
	    public bool Vendido { get; set; }
    }
}
