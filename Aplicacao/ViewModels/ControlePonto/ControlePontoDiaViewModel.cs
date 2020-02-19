using Core.Extensions;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class ControlePontoDiaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public DateTime Data { get; set; }
        public bool Folga { get; set; }
        public bool Falta { get; set; }
        public bool Atraso { get; set; }
        public bool Suspensao { get; set; }
        public bool Atestado { get; set; }
        public bool FaltaJustificada { get; set; }
        public bool AtrasoJustificado { get; set; }
        public string Observacao { get; set; }
        public string HorarioEntrada { get; set; }
        public string HorarioSaidaAlmoco { get; set; }
        public string HorarioRetornoAlmoco { get; set; }
        public string HorarioSaida { get; set; }
        public TimeSpan HorasDiaTime { get; set; }
        public string AdicionalNoturno { get; set; }
        public string HorasDia { get; set; }
        public string HoraExtra { get; set; }
        public string HoraAtraso { get; set; }
        public bool EhFeriado { get; set; }
        public bool EhFerias { get; set; }
        public bool TemHoraExtra => !string.IsNullOrEmpty(HoraExtra) && HoraExtra != "00:00";
        public bool TemAdicionalNoturno => !string.IsNullOrEmpty(AdicionalNoturno) && AdicionalNoturno != "00:00";
        public bool TemIntervaloPedente {
            get
            {
                if(string.IsNullOrEmpty(HorarioSaidaAlmoco) && string.IsNullOrEmpty(HorarioRetornoAlmoco) && CalcularHorasDiaTime() > TimeSpan.Parse("06:05"))
                {
                    return true;
                }

                return false;
            }
        }

        public string Dia => Data.ToShortDateString();

        public List<ControlePontoUnidadeApoioViewModel> UnidadesApoio { get; set; }

        public ControlePontoDiaViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public TimeSpan CalcularHorasDiaTime()
        {
            if(string.IsNullOrEmpty(HorarioSaida) && string.IsNullOrEmpty(HorarioSaidaAlmoco) && string.IsNullOrEmpty(HorarioRetornoAlmoco))
            {
                HorasDiaTime = TimeSpan.Parse("00:00");
            }
            else
            {
                var hoje = DateTime.Now;
                var horarioEntrada = !string.IsNullOrEmpty(HorarioEntrada) ? TimeSpan.Parse(HorarioEntrada) : TimeSpan.Parse("00:00");
                var horarioSaida = !string.IsNullOrEmpty(HorarioSaida) ? TimeSpan.Parse(HorarioSaida) : TimeSpan.Parse("00:00");
                horarioSaida = string.IsNullOrEmpty(HorarioSaida) && !string.IsNullOrEmpty(HorarioRetornoAlmoco) ? TimeSpan.Parse(HorarioRetornoAlmoco) : horarioSaida;

                var entrada = new DateTime(hoje.Year, hoje.Month, hoje.Day, horarioEntrada.Hours, horarioEntrada.Minutes, 0);
                var saida = new DateTime(hoje.Year, hoje.Month, hoje.Day, horarioSaida.Hours, horarioSaida.Minutes, 0);

                if (saida < entrada)
                    saida = saida.AddDays(1);

                HorasDiaTime = saida - entrada;

                HorasDiaTime -= CalcularHorarioAlmoco();
            }

            return HorasDiaTime;
        }

        public void CalcularHorasDia()
        {
            HorasDia = CalcularHorasDiaTime().ToStringTotalHour();
        }

        public TimeSpan CalcularHorarioAlmoco()
        {
            var hoje = DateTime.Now;
            var horarioSaidaAlmoco = !string.IsNullOrEmpty(HorarioSaidaAlmoco) ? TimeSpan.Parse(HorarioSaidaAlmoco) : TimeSpan.Parse("00:00");
            var horarioRetornoAlmoco = !string.IsNullOrEmpty(HorarioRetornoAlmoco) ? TimeSpan.Parse(HorarioRetornoAlmoco) : TimeSpan.Parse("00:00");

            var saidaAlmoco = new DateTime(hoje.Year, hoje.Month, hoje.Day, horarioSaidaAlmoco.Hours, horarioSaidaAlmoco.Minutes, 0);
            var retornoAlmoco = new DateTime(hoje.Year, hoje.Month, hoje.Day, horarioRetornoAlmoco.Hours, horarioRetornoAlmoco.Minutes, 0);

            if (retornoAlmoco < saidaAlmoco)
                retornoAlmoco.AddDays(1);

            return retornoAlmoco - saidaAlmoco;
        }

        public void CalcularHoraExtra(TipoEscalaFuncionario? tipoEscala)
        {
            switch (tipoEscala)
            {
                case TipoEscalaFuncionario.DozeTrintaSeis:
                    {
                        var horasminimas = new TimeSpan(12,0,0);
                        HoraExtra = HorasDiaTime > horasminimas ? (HorasDiaTime - horasminimas).ToStringTotalHour() : "00:00";
                    }
                    break;
                default:
                    {
                        var horasminimas = TimeSpan.Parse("08:00");
                        HoraExtra = HorasDiaTime > horasminimas ? (HorasDiaTime - horasminimas).ToStringTotalHour() : "00:00";
                    }
                    break;
            }
        }

        public void CalcularHoraAtraso()
        {
            if (!Atraso)
            {
                HoraAtraso = "00:00";
            }
            else
            {
                var horasminimas = TimeSpan.Parse("08:00");
                HoraAtraso = HorasDiaTime < horasminimas ? (HorasDiaTime - horasminimas).ToStringTotalHour() : "00:00";
            }
        }

        public void CalcularAdicionalNoturno(TipoEscalaFuncionario? tipoEscala)
        {
            var hoje = DateTime.Now;
            var horarioEntrada = !string.IsNullOrEmpty(HorarioEntrada) ? TimeSpan.Parse(HorarioEntrada) : TimeSpan.Parse("00:00");
            var horarioSaida = !string.IsNullOrEmpty(HorarioSaida) ? TimeSpan.Parse(HorarioSaida) : TimeSpan.Parse("00:00");

            if(horarioEntrada.Hours == 0 || horarioSaida.Hours == 0)
            {
                AdicionalNoturno = TimeSpan.Parse("00:00").ToStringTotalHour();
            }
            else
            {
                var inicioNoturno = new DateTime(hoje.Year, hoje.Month, hoje.Day, 22, 0, 0);

                var amanha = hoje.AddDays(1);
                var fimNoturno = new DateTime(amanha.Year, amanha.Month, amanha.Day, 5, 0, 0);

                var entrada = new DateTime(hoje.Year, hoje.Month, hoje.Day, horarioEntrada.Hours, horarioEntrada.Minutes, 0);
                var saida = new DateTime(hoje.Year, hoje.Month, hoje.Day, horarioSaida.Hours, horarioSaida.Minutes, 0);

                if (saida < entrada)
                    saida = saida.AddDays(1);


                if(tipoEscala != TipoEscalaFuncionario.DozeTrintaSeis)
                    saida = saida > fimNoturno ? fimNoturno : saida;

                var adicionalNoturno = (saida - inicioNoturno);
                adicionalNoturno = adicionalNoturno.Hours > 0 ? adicionalNoturno : TimeSpan.Parse("00:00");
                adicionalNoturno = adicionalNoturno.Hours == 7 ? adicionalNoturno.Add(TimeSpan.Parse("01:00")) : adicionalNoturno;
                AdicionalNoturno = adicionalNoturno.ToStringTotalHour();
            }
        }

        public void AdicionarUnidadesApoio(ControlePontoUnidadeApoioViewModel unidadeApoio)
        {
            if (UnidadesApoio == null)
                UnidadesApoio = new List<ControlePontoUnidadeApoioViewModel>();

            this.UnidadesApoio.Add(unidadeApoio);
        }
    }
}