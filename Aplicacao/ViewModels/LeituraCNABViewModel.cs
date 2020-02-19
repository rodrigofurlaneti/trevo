using System;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class LeituraCNABViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public TipoServico TipoServico { get; set; }
        public string NumeroCNAB { get; set; }

        public DateTime DataVencimentoInicio { get; set; }
        public DateTime? DataVencimentoFim { get; set; }

        public DateTime? DataBaixa { get; set; }

        public string Unidade { get; set; }
        
        public decimal ValorTotal { get; set; }
    }
}