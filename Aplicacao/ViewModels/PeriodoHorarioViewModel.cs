using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class PeriodoHorarioViewModel
    {
        public int Id { get; set; }
        public int TipoHorario { get; set; }
        public string Periodo { get; set; }
        public string Inicio { get; set; }
        public string Fim { get; set; }

        public PeriodoHorarioViewModel()
        {

        }

        public PeriodoHorarioViewModel(PeriodoHorario PeriodoHorario)
        {
            Id = PeriodoHorario.Id;
            TipoHorario = PeriodoHorario.TipoHorario;
            Periodo = PeriodoHorario.Periodo;
            Inicio = PeriodoHorario.Inicio;
            Fim = PeriodoHorario.Fim;

        }

        public PeriodoHorario ToEntity()
        {
            var entidade = new PeriodoHorario
            {
                Id = Id,
                TipoHorario = TipoHorario,
                Periodo = Periodo,
                Inicio = Inicio,
                Fim = Fim
            };

            return entidade;
        }
    }
}
