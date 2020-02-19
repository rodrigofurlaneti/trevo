using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ControlePontoUnidadeApoioViewModel
    {
        public Unidade Unidade { get; set; }
        public DateTime Data { get; set; }
        public string HorarioEntrada { get; set; }
        public string HorarioSaida { get; set; }
        public TipoHoraExtra TipoHoraExtra { get; set; }

        public TimeSpan RetornarHorasTotais()
        {
            if (string.IsNullOrEmpty(HorarioEntrada) || string.IsNullOrEmpty(HorarioSaida))
                return new TimeSpan(0, 0, 0);

            return TimeSpan.Parse(HorarioSaida) - TimeSpan.Parse(HorarioEntrada);
        }
    }
}