using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class PlanoCarreiraViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public string Valor { get; set; }
        public string AnoDe { get; set; }
        public string AnoAte { get; set; }

        public PlanoCarreiraViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}