using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class ControleFeriasPeriodoPermitidoViewModel
    {
        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }
        public string DataDeString => DataDe.ToShortDateString();
        public string DataAteString => DataDe.ToShortDateString();
    }
}