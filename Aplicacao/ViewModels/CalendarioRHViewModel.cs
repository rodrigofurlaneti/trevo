using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class CalendarioRHViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public List<CalendarioRHUnidadeViewModel> CalendarioRHUnidades { get; set; }
        public bool DataFixa { get; set; }
        public bool TodasUnidade { get; set; }

        public CalendarioRHViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}