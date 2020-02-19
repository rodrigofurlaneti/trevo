using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class HorarioUnidadePeriodoHorarioViewModel
    {
        public PeriodoHorarioViewModel PeriodoHorario { get; set; }
        public int HorarioUnidade { get; set; }

        public HorarioUnidadePeriodoHorarioViewModel()
        {
            PeriodoHorario = new PeriodoHorarioViewModel();
        }

        public HorarioUnidadePeriodoHorarioViewModel(HorarioUnidadePeriodoHorario HorarioUnidadePeriodoHorario)
        {
            PeriodoHorario = new PeriodoHorarioViewModel(HorarioUnidadePeriodoHorario?.PeriodoHorario ?? new PeriodoHorario());
            HorarioUnidade = HorarioUnidadePeriodoHorario.HorarioUnidade;
        }

        public HorarioUnidadePeriodoHorario ToEntity()
        {
            return new HorarioUnidadePeriodoHorario
            {
                PeriodoHorario = PeriodoHorario?.ToEntity(),
                HorarioUnidade = HorarioUnidade,
            };
        }

        public HorarioUnidadePeriodoHorario ToEntity(HorarioUnidadePeriodoHorarioViewModel HorarioUnidadePeriodoHorario)
        {
            return new HorarioUnidadePeriodoHorario
            {
                DataInsercao = DateTime.Now,
                PeriodoHorario = HorarioUnidadePeriodoHorario.PeriodoHorario?.ToEntity(),
                HorarioUnidade = HorarioUnidadePeriodoHorario.HorarioUnidade,
            };
        }

        public HorarioUnidadePeriodoHorarioViewModel ToViewModel(HorarioUnidadePeriodoHorario HorarioUnidadePeriodoHorario)
        {
            return new HorarioUnidadePeriodoHorarioViewModel
            {
                PeriodoHorario = new PeriodoHorarioViewModel(HorarioUnidadePeriodoHorario?.PeriodoHorario ?? new PeriodoHorario()),
                HorarioUnidade = HorarioUnidadePeriodoHorario.HorarioUnidade,
            };
        }

        public IList<HorarioUnidadePeriodoHorarioViewModel> ListaPeriodoHorarios(IList<HorarioUnidadePeriodoHorario> HorarioUnidadePeriodoHorarios)
        {
            var HorarioUnidadePeriodoHorariosViewModel = new List<HorarioUnidadePeriodoHorarioViewModel>();
            foreach (var PeriodoHorario in HorarioUnidadePeriodoHorarios)
            {
                HorarioUnidadePeriodoHorariosViewModel.Add(ToViewModel(PeriodoHorario));
            }

            return HorarioUnidadePeriodoHorariosViewModel;
        }

        public IList<HorarioUnidadePeriodoHorario> ListaPeriodoHorarios(IList<HorarioUnidadePeriodoHorarioViewModel> HorarioUnidadePeriodoHorarios)
        {
            var HorarioUnidadePeriodoHorariosViewModel = new List<HorarioUnidadePeriodoHorario>();
            foreach (var PeriodoHorario in HorarioUnidadePeriodoHorarios)
            {
                HorarioUnidadePeriodoHorariosViewModel.Add(ToEntity(PeriodoHorario));
            }

            return HorarioUnidadePeriodoHorariosViewModel;
        }
    }
}
