using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class SolicitacaoPagamentoReembolsoViewModel
    {
        public int? ContasAPagarId { get; set; }
        public int? ContaFinanceiraId { get; set; }
        public int? DepartamentoId { get; set; }
        public TipoContaPagamento? TipoPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}