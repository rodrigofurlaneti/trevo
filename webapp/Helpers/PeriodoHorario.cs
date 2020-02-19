using Aplicacao.ViewModels;
using System.Collections.Generic;

namespace Portal.Helpers
{
    public static class PeriodoHorario
    {
        public static List<PeriodoHorarioViewModel> RetornaPeriodoHorarios(List<HorarioUnidadePeriodoHorarioViewModel> HorarioUnidadePeriodoHorarios)
        {
            var PeriodoHorarios = new List<PeriodoHorarioViewModel>();
            foreach (var HorarioUnidadePeriodoHorario in HorarioUnidadePeriodoHorarios)
            {
                PeriodoHorarios.Add(HorarioUnidadePeriodoHorario.PeriodoHorario);
            }

            return PeriodoHorarios;
        }


        public static List<HorarioUnidadePeriodoHorarioViewModel> RetornaHorarioUnidadePeriodoHorarios(int idHorarioUnidade, List<PeriodoHorarioViewModel> PeriodoHorarios)
        {
            var HorarioUnidadePeriodoHorarios = new List<HorarioUnidadePeriodoHorarioViewModel>();
            foreach (var PeriodoHorario in PeriodoHorarios)
            {
                var HorarioUnidadePeriodoHorario = new HorarioUnidadePeriodoHorarioViewModel();
                HorarioUnidadePeriodoHorario.HorarioUnidade = idHorarioUnidade;
                HorarioUnidadePeriodoHorario.PeriodoHorario = new PeriodoHorarioViewModel();
                HorarioUnidadePeriodoHorario.PeriodoHorario = PeriodoHorario;
                HorarioUnidadePeriodoHorarios.Add(HorarioUnidadePeriodoHorario);
            }

            return HorarioUnidadePeriodoHorarios;
        }
    }
}