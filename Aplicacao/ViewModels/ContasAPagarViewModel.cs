using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class ContasAPagarViewModel
    {
        public int Id { get; set; }
        public ContaFinanceira ContaFinanceira { get; set; }
        public TipoContaPagamento TipoPagamento { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public ContaContabilViewModel ContaContabil { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataCompetencia { get; set; }
        public DateTime DataPagamento { get; set; }
        public DepartamentoViewModel Departamento { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }
        public string ValorTotal { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public int NumeroParcela { get; set; }
        public string Observacoes { get; set; }
        public bool PodePagarEspecie { get; set; }
        public bool ValorSolicitado { get; set; }
        public bool Ignorado { get; set; }
        public StatusContasAPagar StatusConta { get; set; }
        public string CodigoAgrupadorParcela { get; set; }
        public string NumeroRecibo { get; set; }
        public TipoJurosContaPagar TipoJuros { get; set; }
        public string ValorJuros { get; set; }
        public TipoMultaContaPagar TipoMulta { get; set; }
        public string ValorMulta { get; set; }
        public string Contribuinte { get; set; }
        public string CodigoDeBarras { get; set; }
        public List<ContasAPagarItemViewModel> ContaPagarItens { get; set; }

        public TipoDocumentoConta TipoDocumentoConta { get; set; }
        public string NumeroDocumento { get; set; }

        public string ValorFormatado => ValorTotal != null && ValorTotal.ToString().IndexOf(',') != -1
            ? $"R$ {ValorTotal.ToString().Substring(0, ValorTotal.ToString().IndexOf(',') + 3)}"
            : $"R$ {(ValorTotal != null ? ValorTotal.ToString() : "0")})";

        public ContasAPagarViewModel()
        {
            ContaFinanceira = new ContaFinanceira();
            Departamento = new DepartamentoViewModel();
            Fornecedor = new FornecedorViewModel();
            Unidade = new UnidadeViewModel();
            ContaContabil = new ContaContabilViewModel();
        }

        public ContasAPagarViewModel(ContasAPagar entidade)
        {
            Id = entidade.Id;
            ContaFinanceira = entidade.ContaFinanceira ?? new ContaFinanceira();
            TipoPagamento = entidade.TipoPagamento;
            Unidade = entidade.Unidade != null ? new UnidadeViewModel(entidade.Unidade) : new UnidadeViewModel();
            ContaContabil = entidade.ContaContabil != null ? new ContaContabilViewModel(entidade.ContaContabil) : new ContaContabilViewModel();
            DataVencimento = entidade.DataVencimento;
            DataCompetencia = entidade.DataCompetencia.HasValue ? entidade.DataCompetencia : new DateTime(entidade.DataVencimento.Year, entidade.DataVencimento.Month, 1);
            Departamento = entidade.Departamento != null ? new DepartamentoViewModel(entidade.Departamento) : new DepartamentoViewModel();
            Fornecedor = entidade.Fornecedor != null ? new FornecedorViewModel(entidade.Fornecedor) : new FornecedorViewModel();
            ValorTotal = entidade.ValorTotal.ToString("N2");
            FormaPagamento = entidade.FormaPagamento;
            NumeroParcela = entidade.NumeroParcela;
            Observacoes = entidade.Observacoes;
            PodePagarEspecie = entidade.PodePagarEmEspecie;
            ValorSolicitado = entidade.ValorSolicitado;
            StatusConta = entidade.StatusConta;

            CodigoAgrupadorParcela = entidade.CodigoAgrupadorParcela;
            Observacoes = entidade.Observacoes;

            TipoDocumentoConta = entidade.TipoDocumentoConta;
            NumeroDocumento = entidade.NumeroDocumento;

            TipoJuros = entidade.TipoJuros;
            ValorJuros = entidade.ValorJuros.ToString("N2");
            TipoMulta = entidade.TipoMulta;
            ValorMulta = entidade.ValorMulta.ToString("N2");

            Contribuinte = entidade.Contribuinte;
            CodigoDeBarras = entidade.CodigoDeBarras;
            ContaPagarItens = AutoMapper.Mapper.Map<List<ContasAPagarItemViewModel>>(entidade.ContaPagarItens);
        }

        public ContasAPagar ToEntity()
        {
            var entidade = new ContasAPagar
            {
                Id = this.Id,
                ContaFinanceira = this.ContaFinanceira,
                TipoPagamento = this.TipoPagamento,
                Unidade = this.Unidade != null && this.Unidade.Id > 0 ? this.Unidade.ToEntity() : null,
                ContaContabil = this.ContaContabil?.ToEntity(),
                DataVencimento = this.DataVencimento,
                DataCompetencia = this.DataCompetencia.HasValue ? this.DataCompetencia : new DateTime(DataVencimento.Year, DataVencimento.Month, 1),
                Departamento = this.Departamento?.ToEntity(),
                Fornecedor = this.Fornecedor?.ViewModelToEntity(),
                ValorTotal = Convert.ToDecimal(this.ValorTotal.Replace(".", "")),
                FormaPagamento = this.FormaPagamento,
                NumeroParcela = this.NumeroParcela,
                Observacoes = this.Observacoes,
                PodePagarEmEspecie = this.PodePagarEspecie,
                ValorSolicitado = this.ValorSolicitado,
                Ativo = true,
                StatusConta = this.StatusConta,
                CodigoAgrupadorParcela = this.CodigoAgrupadorParcela,
                TipoDocumentoConta = this.TipoDocumentoConta,
                NumeroDocumento = this.NumeroDocumento,
                TipoJuros = this.TipoJuros,
                ValorJuros = decimal.Parse(this.ValorJuros),
                TipoMulta = this.TipoMulta,
                ValorMulta = decimal.Parse(this.ValorMulta),
                Contribuinte = this.Contribuinte,
                CodigoDeBarras = this.CodigoDeBarras,
                ContaPagarItens = AutoMapper.Mapper.Map<List<ContasAPagarItem>>(this.ContaPagarItens)
            };

            return entidade;
        }

        public string FornecedorFormatado => Fornecedor == null
            ? ""
            : !string.IsNullOrEmpty(Fornecedor.Nome)
                ? Fornecedor.Nome
                : !string.IsNullOrEmpty(Fornecedor.NomeFantasia)
                    ? Fornecedor.NomeFantasia
                    : "";
    }
}