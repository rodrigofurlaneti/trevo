using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class TipoPagamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public TipoPagamentoViewModel() { }

        public TipoPagamentoViewModel(TipoPagamento tipoPagamento)
        {
            Id = tipoPagamento.Id;
            DataInsercao = DateTime.Now;
            Codigo = tipoPagamento.Codigo;
            Descricao = tipoPagamento.Descricao;
        }

        public TipoPagamento ToEntity()
        {
            return new TipoPagamento
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Codigo = Codigo,
                Descricao = Descricao
            };
        }

        public TipoPagamentoViewModel ToViewModel(TipoPagamento tipoPagamento)
        {
            return new TipoPagamentoViewModel
            {
                Id = tipoPagamento.Id,
                DataInsercao = tipoPagamento.DataInsercao,
                Codigo = tipoPagamento.Codigo,
                Descricao = tipoPagamento.Descricao
            };
        }
    }
}
