using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aplicacao.ViewModels
{
    public class GeracaoCNABContaPagarViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContaFinanceiraViewModel ContaFinanceira { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public ContaContabilViewModel ContaContabil { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public string Valor { get; set; }
        public string ValorJuros { get; set; }
        public string ValorMulta { get; set; }
        public DateTime? DataVencimento { get; set; }
        public TipoFiltroGeracaoCNAB TipoFiltro { get; set; }

        public List<string> BoletosHtml { get; set; }
        public MemoryStream ArquivoRemessaMemoryStream { get; set; }

        public GeracaoCNABContaPagarViewModel()
        {
        }

        public GeracaoCNABContaPagarViewModel(ContasAPagar contaPagar)
        {
            Id = contaPagar.Id;
            DataInsercao = contaPagar.DataInsercao;
            ContaFinanceira = new ContaFinanceiraViewModel(contaPagar?.ContaFinanceira) ?? new ContaFinanceiraViewModel();
            Unidade = contaPagar?.Unidade != null ? new UnidadeViewModel(contaPagar.Unidade) : new UnidadeViewModel();
            ContaContabil = contaPagar?.ContaContabil != null ? new ContaContabilViewModel(contaPagar.ContaContabil) : new ContaContabilViewModel();
            Fornecedor = contaPagar?.Fornecedor != null ? new FornecedorViewModel(contaPagar.Fornecedor) : new FornecedorViewModel();
            DataVencimento = contaPagar.DataVencimento;
            FormaPagamento = contaPagar.FormaPagamento;
            Valor = contaPagar.ValorTotal.ToString("C");
            ValorJuros = contaPagar.ValorJuros.ToString("C");
            ValorMulta = contaPagar.ValorMulta.ToString("C");
        }

        public ContasAPagar ToEntity() => new ContasAPagar
        {
            Id = Id,
            DataInsercao = DataInsercao,
            ContaFinanceira = ContaFinanceira?.ToEntity(),
            FormaPagamento = FormaPagamento,
            Unidade = Unidade?.ToEntity(),
            ContaContabil = ContaContabil?.ToEntity(),
            Fornecedor = Fornecedor?.ViewModelToEntity(),
            DataVencimento = DataVencimento.Value,
        };
    }
}