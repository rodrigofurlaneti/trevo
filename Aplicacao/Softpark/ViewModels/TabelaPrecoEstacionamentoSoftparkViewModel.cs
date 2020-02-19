using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a Unidade
    /// </summary>
    public class TabelaPrecoEstacionamentoSoftparkViewModel : BaseSoftparkViewModel
    {
        public int TabelaPrecoId { get; set; }
        public TabelaPrecoSoftparkViewModel TabelaPreco { get; set; }
        public int EstacionamentoId { get; set; }
        public EstacionamentoSoftparkViewModel Estacionamento { get; set; }
        public decimal HoraInicial { get; set; }
        public decimal HoraFinal { get; set; }
        public int MinutoInicial { get; set; }
        public int MinutoFinal { get; set; }
        public decimal ValorMaximoDiaria { get; set; }
    }
}
