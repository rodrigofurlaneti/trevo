using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a TabelaPrecoMensalista
    /// </summary>
    public class TabelaPrecoMensalSoftparkViewModel : BaseSoftparkViewModel
    {
        public string Periodo { get; set; }
        public int HoraInicial { get; set; }
        public int HoraFinal { get; set; }
        public int MinutoInicial { get; set; }
        public int MinutoFinal { get; set; }
        public int InSegunda { get; set; }
        public int InTerca { get; set; }
        public int InQuarta { get; set; }
        public int InQuinta { get; set; }
        public int InSexta { get; set; }
        public int InSabado { get; set; }
        public int InDomingo { get; set; }
        public int InFeriado { get; set; }
        public decimal Valor { get; set; }

        public List<TabelaPrecoMensalEstacionamentoSoftparkViewModel> TabelaPrecoMensalEstacionamento { get; set; }

        public TabelaPrecoMensalSoftparkViewModel()
        {
        }

        public TabelaPrecoMensalSoftparkViewModel(TabelaPrecoMensalista tabelaPrecoMensalista)
        {
            Id = tabelaPrecoMensalista.Id;

            var tabelaPrecoUnidade = tabelaPrecoMensalista.TabelaPrecoUnidade.FirstOrDefault();
            var tempoInicio = TimeSpan.Parse(tabelaPrecoUnidade.HorarioInicio);
            var tempoFim = TimeSpan.Parse(tabelaPrecoUnidade.HorarioFim);

            var tabelaPrecoMensalEstacionamentos = new List<TabelaPrecoMensalEstacionamentoSoftparkViewModel>();
            foreach (var tabelaPrecoMensalUnidade in tabelaPrecoMensalista.TabelaPrecoUnidade)
            {
                var estacionamento = new EstacionamentoSoftparkViewModel(tabelaPrecoMensalUnidade.Unidade);
                tabelaPrecoMensalEstacionamentos.Add(new TabelaPrecoMensalEstacionamentoSoftparkViewModel
                {
                    Id = tabelaPrecoMensalUnidade.Id,
                    DataInsercao = tabelaPrecoMensalUnidade.DataInsercao,
                    TabelaPrecoMensal = this,
                    TabelaPrecoMensalId = this.Id,
                    Estacionamento = estacionamento,
                    EstacionamentoId = estacionamento.Id,
                    DiasParaCorte = tabelaPrecoMensalUnidade.DiasParaCorte
            });
            }

            Periodo = tabelaPrecoMensalista.Nome;
            HoraInicial = tempoInicio.Hours;
            HoraFinal = tempoFim.Hours;
            MinutoInicial = tempoInicio.Minutes;
            MinutoFinal = tempoFim.Minutes;
            InSegunda = 1;
            InTerca = 1;
            InQuarta = 1;
            InQuinta = 1;
            InSexta = 1;
            InSabado = 1;
            InDomingo = 1;
            InFeriado = 1;
            Valor = tabelaPrecoMensalista.Valor;
            DataInsercao = tabelaPrecoMensalista.DataInsercao;
            TabelaPrecoMensalEstacionamento = tabelaPrecoMensalEstacionamentos;
        }
    }
}
