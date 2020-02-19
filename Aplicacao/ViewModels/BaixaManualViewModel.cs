using System;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class BaixaManualViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContaFinanceiraViewModel ContaFinanceira { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public TipoServico TipoServico { get; set; }
        public DateTime DataVencimentoInicio { get; set; }
        public DateTime DataVencimentoFim { get; set; }

        public DateTime? DataBaixa { get; set; }
        
        //Controle de Tela:
        public string Documento { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataCompetencia { get; set; }
        public decimal ValorContrato { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal ValorJuros { get; set; }
        public string ValorRecebido { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorAReceber { get; set; }
        public bool Realizabaixa { get; set; }

        public string NumeroRecibo { get; set; }
        public DateTime DataPagamento { get; set; }
        public string ValorPago { get; set; }

        public FormaPagamento FormaPagamento { get; set; }
        public ContaContabil ContaContabil { get; set; }

        public StatusLancamentoCobranca StatusLancamentoCobranca { get; set; }

        public decimal? ValorDivergente { get; set; }
        public TipoDescontoAcrescimo TipoDescontoAcrescimo { get; set; }
        public string Justificativa { get; set; }

        public BaixaManualViewModel()
        {
            ContaFinanceira = new ContaFinanceiraViewModel();
            Cliente = new ClienteViewModel();
        }

        public BaixaManualViewModel(LancamentoCobranca lancamentoCobranca)
        {
            Id = lancamentoCobranca.Id;
            DataInsercao = lancamentoCobranca.DataInsercao;
            DataVencimento = lancamentoCobranca.DataVencimento;
            DataCompetencia = lancamentoCobranca.DataCompetencia;
            StatusLancamentoCobranca = lancamentoCobranca.StatusLancamentoCobranca;
            DataBaixa = lancamentoCobranca.DataBaixa;
            ValorContrato = lancamentoCobranca.ValorContrato;
            ValorMulta = lancamentoCobranca.ValorMulta;
            ValorJuros = lancamentoCobranca.ValorJuros;
            ValorAReceber = lancamentoCobranca.ValorAReceber;
            ValorRecebido = lancamentoCobranca.ValorRecebido;
            ValorTotal = lancamentoCobranca.ValorTotal;

            ContaFinanceira = new ContaFinanceiraViewModel(lancamentoCobranca?.ContaFinanceira) ?? new ContaFinanceiraViewModel();
            Cliente = new ClienteViewModel(lancamentoCobranca?.Cliente) ?? new ClienteViewModel();
            
            TipoServico = lancamentoCobranca.TipoServico;
        }

        public LancamentoCobranca ToEntity() => new LancamentoCobranca
        {
            Id = Id,
            DataInsercao = DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            DataVencimento = DataVencimento,
            DataCompetencia = DataCompetencia,
            StatusLancamentoCobranca = StatusLancamentoCobranca,
            DataBaixa = DataBaixa,
            ValorContrato = ValorContrato,
            ValorMulta = ValorMulta,
            ValorJuros = ValorJuros,

            ContaFinanceira = ContaFinanceira.ToEntity(),
            Cliente = Cliente.ToEntity(),
            
            TipoServico = TipoServico
        };

    }
}