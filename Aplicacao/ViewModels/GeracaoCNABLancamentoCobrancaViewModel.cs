using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class GeracaoCNABLancamentoCobrancaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public BancoViewModel Banco { get; set; }
        public ContaFinanceiraViewModel ContaFinanceira { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public TipoServico TipoServico { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public DateTime DataVencimentoInicio { get; set; }
        public DateTime DataVencimentoFim { get; set; }
        public IEnumerable<IGrouping<Unidade, LancamentoCobranca>> LancamentosAgrupados { get; set; }

        public DateTime? DataBaixa { get; set; }
        public DateTime? DataCompetencia { get; set; }

        //Controle de Tela:
        public string CPF { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorContrato { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal ValorJuros { get; set; }
        public string ValorRecebido { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorAReceber { get; set; }
        public bool Realizabaixa { get; set; }
        public string DataDEFiltro { get; set; }
        public string DataATEFiltro { get; set; }

        public List<string> BoletosHtml { get; set; }
        public MemoryStream ArquivoRemessaMemoryStream { get; set; }
        public byte[] DadosPDF { get; set; }

        public StatusLancamentoCobranca StatusLancamentoCobranca { get; set; }
        public TipoFiltroGeracaoCNAB TipoFiltroGeracaoCNAB { get; set; }

        public FuncionamentoViewModel Supervisor { get; set; }
        
        public GeracaoCNABLancamentoCobrancaViewModel()
        {
            Banco = new BancoViewModel();
            ContaFinanceira = new ContaFinanceiraViewModel();
            Cliente = new ClienteViewModel();
            Unidade = new UnidadeViewModel();
        }

        public GeracaoCNABLancamentoCobrancaViewModel(LancamentoCobranca lancamentoCobranca)
        {
            Id = lancamentoCobranca.Id;
            DataInsercao = lancamentoCobranca.DataInsercao;
            DataGeracao = lancamentoCobranca.DataGeracao;
            DataVencimento = lancamentoCobranca.DataVencimento;
            DataCompetencia = lancamentoCobranca.DataCompetencia;
            DataBaixa = lancamentoCobranca.DataBaixa;
            ValorContrato = lancamentoCobranca.ValorContrato;
            ValorMulta = lancamentoCobranca.ValorMulta;
            ValorJuros = lancamentoCobranca.ValorJuros;
            ValorAReceber = lancamentoCobranca.ValorAReceber;
            ValorRecebido = lancamentoCobranca.ValorRecebido;
            ValorTotal = lancamentoCobranca.ValorTotal;

            Banco = new BancoViewModel(lancamentoCobranca?.ContaFinanceira?.Banco) ?? new BancoViewModel();
            ContaFinanceira = new ContaFinanceiraViewModel(lancamentoCobranca?.ContaFinanceira) ?? new ContaFinanceiraViewModel();
            Cliente = new ClienteViewModel(lancamentoCobranca?.Cliente) ?? new ClienteViewModel();
            Unidade = new UnidadeViewModel(lancamentoCobranca?.Unidade) ?? new UnidadeViewModel();

            TipoServico = lancamentoCobranca.TipoServico;
        }

        public LancamentoCobranca ToEntity() => new LancamentoCobranca
        {
            Id = Id,
            DataInsercao = DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            DataGeracao = DataGeracao,
            DataVencimento = DataVencimento,
            DataCompetencia = DataCompetencia,
            DataBaixa = DataBaixa,
            ValorContrato = ValorContrato,
            ValorMulta = ValorMulta,
            ValorJuros = ValorJuros,

            ContaFinanceira = ContaFinanceira.ToEntity(),
            Cliente = Cliente.ToEntity(),
            Unidade = Unidade.ToEntity(),

            TipoServico = TipoServico
        };

    }
}