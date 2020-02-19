using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoAvulsoPeriodoViewModel
    {
        public List<TipoPeriodo> ListaPeriodo { get; set; }
        public List<TipoPeriodo> ListaPeriodoSelecionado { get; set; }

        public TabelaPrecoAvulsoPeriodoViewModel()
        {
            ListaPeriodo = new List<TipoPeriodo>();
            ListaPeriodo = Enum.GetValues(typeof(TipoPeriodo)).Cast<TipoPeriodo>().ToList();
            ListaPeriodoSelecionado = new List<TipoPeriodo>();
        }
    }
}