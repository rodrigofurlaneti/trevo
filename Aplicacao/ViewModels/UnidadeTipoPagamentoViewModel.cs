using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class UnidadeTipoPagamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoPagamentoViewModel TipoPagamento { get; set; }
        public int Unidade { get; set; }

        public UnidadeTipoPagamentoViewModel()
        {

        }

        public UnidadeTipoPagamentoViewModel(Tipospagamentos unidadeTipoPagamento)
        {
            Id = unidadeTipoPagamento.Id;
            DataInsercao = unidadeTipoPagamento.DataInsercao;
            Unidade = unidadeTipoPagamento.Unidade;
            TipoPagamento = new TipoPagamentoViewModel(unidadeTipoPagamento?.TipoPagamento ?? new TipoPagamento());
        }

        public Tipospagamentos ToEntity()
        {
            return new Tipospagamentos
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Unidade = Unidade,
                TipoPagamento = TipoPagamento?.ToEntity()
            };
        }

        public UnidadeTipoPagamentoViewModel ToViewModel(Tipospagamentos unidadeTipoPagamento)
        {
            return new UnidadeTipoPagamentoViewModel
            {
                Id = unidadeTipoPagamento.Id,
                DataInsercao = unidadeTipoPagamento.DataInsercao,
                Unidade = unidadeTipoPagamento.Unidade,
                TipoPagamento = new TipoPagamentoViewModel(unidadeTipoPagamento?.TipoPagamento ?? new TipoPagamento())
            };
           
        }
    }
}
