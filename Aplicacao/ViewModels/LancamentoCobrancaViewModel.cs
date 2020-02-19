using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class LancamentoCobrancaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContaFinanceiraViewModel ContaFinanceira { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public TipoServico TipoServico { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataCompetencia { get; set; }
        public DateTime? DataBaixa { get; set; }
        public DateTime? DataUltimoPagamento { get; set; }
        public string ValorContrato { get; set; }
        public string ValorMulta { get; set; }
        public TipoValor TipoValorMulta { get; set; }
        public string ValorJuros { get; set; }
        public TipoValor TipoValorJuros { get; set; }
        public string CiaSeguro { get; set; }
        public string ValorTotal { get; set; }
        public string ValorRecebido { get; set; }
        public bool PossueCnab { get; set; }

        public string ValorTotalPago { get; set; }

        public StatusLancamentoCobranca StatusLancamentoCobranca { get; set; }

        public string Observacao { get; set; }

        public string NumeroRecibo { get; set; }
        public string NumeroContratoOuRegistro { get; set; }

        public ChaveValorCategoriaViewModel CobrancaTipoServico { get; set; }
        public IList<ChaveValorCategoriaViewModel> ListaCobrancaTipoServico { get; set; }

        public int NumeroContratoPesquisa { get; set; }

        public LancamentoCobrancaViewModel()
        {
            ContaFinanceira = new ContaFinanceiraViewModel();
            Cliente = new ClienteViewModel();
            Unidade = new UnidadeViewModel();
            CobrancaTipoServico = new ChaveValorCategoriaViewModel();
        }

        public LancamentoCobrancaViewModel(LancamentoCobranca lancamentoCobranca)
        {
            Id = lancamentoCobranca.Id;
            DataInsercao = lancamentoCobranca.DataInsercao;
            ContaFinanceira = lancamentoCobranca?.ContaFinanceira != null ? new ContaFinanceiraViewModel(lancamentoCobranca.ContaFinanceira) : new ContaFinanceiraViewModel();
            Cliente = lancamentoCobranca.Cliente != null && lancamentoCobranca.Cliente.Id > 0 ? new ClienteViewModel(lancamentoCobranca?.Cliente) : null;
            Unidade = lancamentoCobranca?.Unidade != null ? new UnidadeViewModel(lancamentoCobranca.Unidade) : new UnidadeViewModel();
            DataGeracao = lancamentoCobranca.DataGeracao;
            DataVencimento = lancamentoCobranca.DataVencimento;
            DataCompetencia = lancamentoCobranca.DataCompetencia.HasValue ? lancamentoCobranca.DataCompetencia : new DateTime(lancamentoCobranca.DataVencimento.Year, lancamentoCobranca.DataVencimento.Month, 1);
            DataBaixa = lancamentoCobranca.DataBaixa;
            ValorContrato = lancamentoCobranca.ValorContrato.ToString("0.00");
            TipoValorMulta = lancamentoCobranca.TipoValorMulta;
            ValorMulta = lancamentoCobranca.ValorMulta.ToString("0.00");
            TipoValorJuros = lancamentoCobranca.TipoValorJuros;
            ValorJuros = lancamentoCobranca.ValorJuros.ToString("0.00");
            ValorRecebido = lancamentoCobranca.ValorRecebido;
            TipoServico = lancamentoCobranca.TipoServico;
            StatusLancamentoCobranca = lancamentoCobranca.StatusLancamentoCobranca;
            CiaSeguro = lancamentoCobranca.CiaSeguro;
            ValorTotal = lancamentoCobranca.ValorTotal.ToString();
            ValorTotalPago = (lancamentoCobranca.Recebimento?.Pagamentos?.Sum(x => x.ValorPago) ?? 0).ToString("C2");
            DataUltimoPagamento = lancamentoCobranca?.Recebimento?.Pagamentos.LastOrDefault()?.DataPagamento;
            NumeroRecibo = lancamentoCobranca?.Recebimento?.Pagamentos.LastOrDefault()?.NumeroRecibo;
            NumeroContratoOuRegistro = lancamentoCobranca?.NumerosContratos;
            PossueCnab = lancamentoCobranca.PossueCnab;
            Observacao = lancamentoCobranca.Observacao;
        }

        public LancamentoCobranca ToEntity()
        {
            var unidade = Unidade.ToEntity();

            return new LancamentoCobranca
            {
                Id = Id,
                DataInsercao = DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
                ContaFinanceira = ContaFinanceira.ToEntity(),
                Cliente = new Cliente { Id = Cliente.Id },
                Unidade = unidade.Id == 0 ? null : unidade,
                DataGeracao = DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataGeracao,
                DataVencimento = DataVencimento,
                DataCompetencia = DataCompetencia.HasValue ? DataCompetencia : new DateTime(DataVencimento.Year, DataVencimento.Month, 1),
                DataBaixa = DataBaixa,
                ValorContrato = Convert.ToDecimal(ValorContrato),
                TipoValorMulta = TipoValorMulta,
                ValorMulta = Convert.ToDecimal(ValorMulta),
                TipoValorJuros = TipoValorJuros,
                ValorJuros = Convert.ToDecimal(ValorJuros),
                TipoServico = TipoServico,
                StatusLancamentoCobranca = StatusLancamentoCobranca,
                CiaSeguro = CiaSeguro,
                PossueCnab = PossueCnab,
                Observacao = Observacao
            };
        }
    }
}