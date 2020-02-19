using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class TipoOcorrenciaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public string Percentual { get; set; }

        public TipoOcorrenciaViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}