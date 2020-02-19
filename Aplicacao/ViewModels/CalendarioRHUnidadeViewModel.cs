using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class CalendarioRHUnidadeViewModel
    {
        public UnidadeViewModel Unidade { get; set; }

        public CalendarioRHUnidadeViewModel()
        {
        }

        public CalendarioRHUnidadeViewModel(UnidadeViewModel unidade)
        {
            Unidade = unidade;
        }
    }
}