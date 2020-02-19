using Core.Extensions;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ControlePontoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public FuncionarioViewModel Supervisor { get; set; }
        public FuncionarioViewModel Funcionario { get; set; }
        public List<ControlePontoDiaViewModel> ControlePontoDias { get; set; }
        public List<ControlePontoDiaViewModel> ControlePontoDiasDoMes { get; set; }
        public int IntervalosPendentes => ControlePontoDiasDoMes?.Count(x => x.TemIntervaloPedente) ?? 0;
        public int TotalFalta => ControlePontoDiasDoMes?.Count(x => x.Falta && !x.FaltaJustificada && !x.Atestado) ?? 0;
        public string TotalAtraso => CalcularTotalAtraso().ToStringTotalHour();
        public string TotalHoraExtraSessentaCinco => CalcularTotalHoraExtraSessentaCincoDia().ToStringTotalHour();

        public string TotalHoraExtraCem => CalcularTotalHoraExtraCemDia().ToStringTotalHour();

        public int TotalFeriadosTrabalhados => ControlePontoDiasDoMes?.Where(x => x.EhFeriado && x.HorasDiaTime.Ticks > 0)?.Count() ?? 0;

        public string TotalAdicionalNoturno => CalcularTotalAdicionalNoturno().ToStringTotalHour();

        public ControlePontoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        private TimeSpan CalcularTotalAdicionalNoturno()
        {
            return new TimeSpan(ControlePontoDiasDoMes?.Where(x => x.TemAdicionalNoturno)?
                                                            .Sum(x => TimeSpan.Parse(x.AdicionalNoturno).Ticks) ?? 0);
        }

        private TimeSpan CalcularTotalHoraExtra()
        {
            switch (Funcionario.TipoEscala)
            {
                case TipoEscalaFuncionario.DozeTrintaSeis:
                    return CalcularTotalHoraExtraDozeTrintaSeis();
                case TipoEscalaFuncionario.Compensacao:
                    return CalcularTotalHoraExtraCompensacao();
                default:
                    return CalcularTotalHoraExtraPadrao();
            }
        }

        private TimeSpan CalcularTotalHoraExtraDozeTrintaSeis()
        {
            return new TimeSpan(ControlePontoDiasDoMes.Where(x => x.TemHoraExtra).Sum(x => TimeSpan.Parse(x.HoraExtra).Ticks));
        }

        private TimeSpan CalcularTotalHoraExtraCompensacao()
        {
            var cargaHorariaSemanal = new TimeSpan(44, 0, 0);

            var totalHorasExtras = new TimeSpan(0);
            var horasTrabalhoSemanal = new TimeSpan(0);
            var horasExtraSemanal = new TimeSpan(0);
            foreach (var dia in ControlePontoDiasDoMes)
            {
                //Verifica se é domingo para zerar o calculo da semana e somar no total
                if (dia.Data.DayOfWeek == DayOfWeek.Monday)
                {
                    horasExtraSemanal = horasTrabalhoSemanal > cargaHorariaSemanal ? horasTrabalhoSemanal - cargaHorariaSemanal : new TimeSpan(0);

                    totalHorasExtras += horasExtraSemanal;
                    horasTrabalhoSemanal = new TimeSpan(0);
                }

                horasTrabalhoSemanal += dia.HorasDiaTime;
            }

            return new TimeSpan(totalHorasExtras.Ticks);
        }

        private TimeSpan CalcularTotalHoraExtraPadrao()
        {
            var cargaHorariaSemanal = new TimeSpan(44, 0, 0);

            var totalHorasExtras = new TimeSpan(0);
            var horasTrabalhoSemanal = new TimeSpan(0);
            var horasExtraDiarias = new TimeSpan(0);
            var horasExtraSemanal = new TimeSpan(0);
            foreach (var dia in ControlePontoDiasDoMes)
            {
                //Verifica se é domingo para zerar o calculo da semana e somar no total
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

            return new TimeSpan(totalHorasExtras.Ticks);
        }

        private TimeSpan CalcularTotalHoraExtraCemDia()
        {
            return new TimeSpan(ControlePontoDiasDoMes?.Where(x => x.TemHoraExtra && x.EhFeriado)?
                                          .Sum(x => TimeSpan.Parse(x.HoraExtra).Ticks) ?? 0);
        }

        private TimeSpan CalcularTotalHoraExtraSessentaCincoDia()
        {
            return CalcularTotalHoraExtra() - CalcularTotalHoraExtraCemDia();
        }

        private long CalcularTotalHoraExtraCemControlePontoUnidadeApoio()
        {
            return ControlePontoDiasDoMes?.Where(x => x.UnidadesApoio != null)?.SelectMany(x => x.UnidadesApoio)?.Where(x => x.TipoHoraExtra == TipoHoraExtra.Cem)?
                                                                     .Sum(x => x.RetornarHorasTotais().Ticks) ?? 0;
        }

        private long CalcularTotalHoraExtraSessentaCincoControlePontoUnidadeApoio()
        {
            return ControlePontoDiasDoMes?.Where(x => x.UnidadesApoio != null)?.SelectMany(x => x.UnidadesApoio)?.Where(x => x.TipoHoraExtra == TipoHoraExtra.SessentaCinco)?
                                                                     .Sum(x => x.RetornarHorasTotais().Ticks) ?? 0;
        }

        private TimeSpan CalcularTotalAtraso()
        {
            return new TimeSpan(ControlePontoDiasDoMes?.Where(x => x.Atraso && !string.IsNullOrEmpty(x.HorarioSaida) && !x.AtrasoJustificado && !x.Atestado)?
                                                .Sum(x => TimeSpan.Parse(x.HoraAtraso).Ticks) ?? 0);
        }
    }
}