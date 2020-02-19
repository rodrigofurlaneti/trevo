using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class FuncionarioIntervaloDozeTrintaSeisViewModel
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string DataInicialString => DataInicial.ToShortDateString();
        public string DataFinalString => DataFinal.ToShortDateString();
    }
}