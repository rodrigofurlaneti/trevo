using Entidade;
using Entidade.Uteis;
using System;
using System.Data.SqlTypes;

namespace Aplicacao.ViewModels
{
    public class PagamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public int NossoNumero { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorPago { get; set; }
        public bool StatusPagamento { get; set; }
        public StatusEmissao StatusEmissao { get; set; }

        public DateTime DataEnvio { get; set; }

        public ContaContabil ContaContabil { get; set; }

        public TipoDescontoAcrescimo? TipoDescontoAcrescimo { get; set; }
        public decimal? ValorDivergente { get; set; }
        public string Justificativa { get; set; }

        public PagamentoViewModel()
        {
            DataInsercao = DateTime.Now;
            ContaContabil = new ContaContabil();
        }

        public PagamentoViewModel(Pagamento pagamento)
        {
            Id = pagamento?.Id ?? 0;
            DataInsercao = pagamento?.DataInsercao ?? DateTime.Now;
            NossoNumero = pagamento?.NossoNumero ?? 0;
            DataPagamento = pagamento?.DataPagamento ?? SqlDateTime.MinValue.Value;
            ValorPago = pagamento?.ValorPago ?? 0;
            TipoDescontoAcrescimo = pagamento.TipoDescontoAcrescimo;
            ValorDivergente = pagamento.ValorDivergente;
            Justificativa = pagamento.Justificativa;
            StatusPagamento = pagamento.StatusPagamento;
            StatusEmissao = StatusEmissao;
            DataEnvio = DataEnvio;
            ContaContabil = ContaContabil;

        }

        public Pagamento ToEntity() => new Pagamento()
        {
            Id = Id,
            DataInsercao = DataInsercao < SqlDateTime.MinValue.Value ? DateTime.Now : DataInsercao,
            NossoNumero = NossoNumero,
            DataPagamento = DataPagamento,
            ValorPago = ValorPago,
            TipoDescontoAcrescimo = TipoDescontoAcrescimo,
            ValorDivergente = ValorDivergente,
            Justificativa = Justificativa,
            StatusPagamento = StatusPagamento,
            StatusEmissao = StatusEmissao,
            DataEnvio = DataEnvio,
            ContaContabil = ContaContabil
        };
    }
}
