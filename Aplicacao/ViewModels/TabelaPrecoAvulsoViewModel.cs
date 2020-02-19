using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoAvulsoViewModel
    {
        public int Id { get; set; }
        public StatusSolicitacao Status { get; set; }
        public string NomeTabela { get; set; }
        public int Numero { get; set; }
        public int TempoToleranciaPagamento { get; set; }
        public int TempoToleranciaDesistencia { get; set; }
        public TabelaPrecoAvulsoPeriodoViewModel Periodo { get; set; }
        public string HoraInicioVigencia { get; set; }
        public string HoraFimVigencia { get; set; }
        public decimal ValorDiaria { get; set; }
        public bool HoraAdicional { get; set; }
        public bool Padrao { get; set; }
        public int? QuantidadeHoraAdicional { get; set; }
        public string ValorHoraAdicionalString { get; set; }
        public decimal ValorHoraAdicional => string.IsNullOrEmpty(ValorHoraAdicionalString) ? 0 : Convert.ToDecimal(ValorHoraAdicionalString);
        public string DescricaoHoraValor { get; set; }
        public List<TabelaPrecoAvulsoHoraValorViewModel> ListaHoraValor { get; set; }
        public List<TabelaPrecoAvulsoUnidadeViewModel> ListaUnidade { get; set; }
        public IList<TabelaPrecoMensalistaNotificacaoViewModel> Notificacoes { get; set; }

        public TabelaPrecoAvulsoViewModel()
        {
            Periodo = new TabelaPrecoAvulsoPeriodoViewModel();
            ListaHoraValor = new List<TabelaPrecoAvulsoHoraValorViewModel>();
            ListaUnidade = new List<TabelaPrecoAvulsoUnidadeViewModel>();
        }

        public TabelaPrecoAvulsoViewModel(TabelaPrecoAvulso entidade)
        {
            if (entidade != null)
            {
                Id = entidade.Id;
                Status = entidade.Status;
                NomeTabela = entidade.Nome;
                Numero = entidade.Numero;
                TempoToleranciaPagamento = entidade.TempoToleranciaPagamento;
                TempoToleranciaDesistencia = entidade.TempoToleranciaDesistencia;
                HoraInicioVigencia = entidade.HoraInicioVigencia;
                HoraFimVigencia = entidade.HoraFimVigencia;
                HoraAdicional = entidade.HoraAdicional;
                Padrao = entidade.Padrao;
                if (HoraAdicional)
                {
                    QuantidadeHoraAdicional = entidade.QuantidadeHoraAdicional == 0 ? null : (int?)entidade.QuantidadeHoraAdicional;
                    ValorHoraAdicionalString = entidade.ValorHoraAdicional == 0 ? string.Empty : entidade.ValorHoraAdicional.ToString("0.00"); ;
                }
                DescricaoHoraValor = entidade.DescricaoHoraValor;
                Notificacoes = entidade?.Notificacoes?.Select(x => new TabelaPrecoMensalistaNotificacaoViewModel())?.ToList() ?? new List<TabelaPrecoMensalistaNotificacaoViewModel>();
            }
        }

        public TabelaPrecoAvulso ToEntity()
        {
            var entidade = new TabelaPrecoAvulso
            {
                Id = Id,
                Status = Status,
                Nome = NomeTabela,
                Numero = Numero,
                TempoToleranciaPagamento = TempoToleranciaPagamento,
                TempoToleranciaDesistencia = TempoToleranciaDesistencia,
                HoraInicioVigencia = HoraInicioVigencia,
                HoraFimVigencia = HoraFimVigencia,
                HoraAdicional = HoraAdicional,
                Padrao = Padrao,
                DescricaoHoraValor = DescricaoHoraValor
            };

            if (entidade.HoraAdicional)
            {
                entidade.QuantidadeHoraAdicional = QuantidadeHoraAdicional.HasValue ? QuantidadeHoraAdicional.Value : 0;
                entidade.ValorHoraAdicional = ValorHoraAdicional;
            }

            return entidade;
        }
    }
}