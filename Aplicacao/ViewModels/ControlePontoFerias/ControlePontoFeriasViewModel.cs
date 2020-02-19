using Core.Extensions;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ControlePontoFeriasViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public FuncionarioViewModel Funcionario { get; set; }
        public List<ControlePontoFeriasDiaViewModel> ControlePontoFeriasDias { get; set; }
        public List<ControlePontoFeriasDiaViewModel> ControlePontoFeriasDiasDoMes { get; set; }
        public int IntervalosPendentes => ControlePontoFeriasDiasDoMes?.Count(x => x.TemIntervaloPedente) ?? 0;
        public int TotalFalta => ControlePontoFeriasDiasDoMes?.Count(x => x.Falta && !x.FaltaJustificada && !x.Atestado) ?? 0;
        public string TotalAtraso => new TimeSpan(CalcularTotalAtraso()).ToStringTotalHour();
        public string TotalHoraExtraSessentaCinco => new TimeSpan(CalcularTotalHoraExtraSessentaCincoDia()).ToStringTotalHour();

        public string TotalHoraExtraCem => new TimeSpan(CalcularTotalHoraExtraCemDia()).ToStringTotalHour();

        public int TotalFeriadosTrabalhados => ControlePontoFeriasDiasDoMes?.Where(x => x.EhFeriado && x.HorasDiaTime.Ticks > 0)?.Count() ?? 0;

        public string TotalAdicionalNoturno => new TimeSpan(ControlePontoFeriasDiasDoMes?
                                                                    .Where(x => x.TemAdicionalNoturno)?
                                                                    .Sum(x => TimeSpan.Parse(x.AdicionalNoturno).Ticks) ?? 0).ToStringTotalHour();

        public ControlePontoFeriasViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        private long CalcularTotalHoraExtra()
        {
            var cargaHorariaSemanal = new TimeSpan(44, 0, 0);

            var totalHorasExtras = new TimeSpan(0);
            var horasTrabalhoSemanal = new TimeSpan(0);
            var horasExtraDiarias = new TimeSpan(0);
            var horasExtraSemanal = new TimeSpan(0);
            if(ControlePontoFeriasDiasDoMes != null)
            {
                foreach (var dia in ControlePontoFeriasDiasDoMes)
                {
                    if (dia.Data.DayOfWeek == DayOfWeek.Monday)
                    {
                        horasExtraSemanal = horasTrabalhoSemanal > cargaHorariaSemanal ? horasTrabalhoSemanal - cargaHorariaSemanal : new TimeSpan(0);

                        var horasMaior = horasExtraSemanal > horasExtraDiarias ? horasExtraSemanal : horasExtraDiarias;
                        totalHorasExtras += horasMaior;
                        horasTrabalhoSemanal = new TimeSpan(0);
                        horasExtraDiarias = new TimeSpan(0);
                    }

                    horasTrabalhoSemanal += dia.HorasDiaTime;
                    horasExtraDiarias += dia.TemHoraExtra ? TimeSpan.Parse(dia.HoraExtra) : new TimeSpan(0);
                }
            }

            return totalHorasExtras.Ticks;
        }

        private long CalcularTotalHoraExtraCemDia()
        {
            return ControlePontoFeriasDiasDoMes?.Where(x => x.TemHoraExtra && x.EhFeriado)?
                                          .Sum(x => TimeSpan.Parse(x.HoraExtra).Ticks) ?? 0;
        }

        private long CalcularTotalHoraExtraSessentaCincoDia()
        {
            return CalcularTotalHoraExtra() - CalcularTotalHoraExtraCemDia();
        }

        private long CalcularTotalHoraExtraCemControlePontoFeriasUnidadeApoio()
        {
            return ControlePontoFeriasDiasDoMes?.Where(x => x.UnidadesApoio != null)?.SelectMany(x => x.UnidadesApoio)?.Where(x => x.TipoHoraExtra == TipoHoraExtra.Cem)?
                                                                     .Sum(x => x.RetornarHorasTotais().Ticks) ?? 0;
        }

        private long CalcularTotalHoraExtraSessentaCincoControlePontoFeriasUnidadeApoio()
        {
            return ControlePontoFeriasDiasDoMes?.Where(x => x.UnidadesApoio != null)?.SelectMany(x => x.UnidadesApoio)?.Where(x => x.TipoHoraExtra == TipoHoraExtra.SessentaCinco)?
                                                                     .Sum(x => x.RetornarHorasTotais().Ticks) ?? 0;
        }

        private long CalcularTotalAtraso()
        {
            return ControlePontoFeriasDiasDoMes?.Where(x => x.Atraso && !string.IsNullOrEmpty(x.HorarioSaida) && !x.AtrasoJustificado && !x.Atestado)?
                                                .Sum(x => TimeSpan.Parse(x.HoraAtraso).Ticks) ?? 0;
        }
    }
}