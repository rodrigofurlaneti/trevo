using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class PedidoLocacaoLancamentoAdicionalViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public int PedidoLocacao { get; set; }
        public string Descricao { get; set; }

        public string Valor { get; set; }

        public string ValorFormatado
        {
            get => !string.IsNullOrEmpty(Valor) ? decimal.Parse(Valor.Replace("R$", "")).ToString("C") : string.Empty;
        }

        public bool Ativo { get; set; }

        public PedidoLocacaoLancamentoAdicionalViewModel()
        {
        }

        public PedidoLocacaoLancamentoAdicionalViewModel(PedidoLocacaoLancamentoAdicional PedidoLocacao)
        {
            Id = PedidoLocacao.Id;

            Descricao = PedidoLocacao.Descricao;
            Valor = PedidoLocacao.Valor.ToString();
            Ativo = PedidoLocacao.Ativo;
        }

        public PedidoLocacaoLancamentoAdicional ToEntity()
        {
            var entidade = new PedidoLocacaoLancamentoAdicional
            {
                Id = Id,
                Descricao = Descricao,
                Valor = Convert.ToDecimal(Valor),
                Ativo = Ativo
            };

            return entidade;
        }
    }
}