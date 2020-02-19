using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class ContaFinanceiraViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public Banco Banco { get; set; }
        public string Descricao { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }

        public string Convenio { get; set; }
        public string ConvenioPagamento { get; set; }
        public string Carteira { get; set; }
        public string CodigoTransmissao { get; set; }

        public EmpresaViewModel Empresa { get; set; }

        public bool ContaPadrao { get; set; }

        public ContaFinanceiraViewModel()
        {
            Banco = new Banco();
        }

        public ContaFinanceiraViewModel(ContaFinanceira contaFinanceira)
        {
            Id = contaFinanceira.Id;
            DataInsercao = contaFinanceira.DataInsercao;
            Descricao = contaFinanceira.Descricao;
            Agencia = contaFinanceira.Agencia;
            DigitoAgencia = contaFinanceira.DigitoAgencia;
            Conta = contaFinanceira.Conta;
            DigitoConta = contaFinanceira.DigitoConta;
            Cpf = contaFinanceira.Cpf;
            Cnpj = contaFinanceira.Cnpj;

            Convenio = contaFinanceira.Convenio;
            Carteira = contaFinanceira.Carteira;
            CodigoTransmissao = contaFinanceira.CodigoTransmissao;
            ContaPadrao = contaFinanceira.ContaPadrao;

            Banco = contaFinanceira?.Banco ?? new Banco();
            Empresa = contaFinanceira.Empresa != null ? new EmpresaViewModel(contaFinanceira.Empresa) : new EmpresaViewModel();

            ConvenioPagamento = contaFinanceira.ConvenioPagamento;
        }

        public ContaFinanceira ToEntity() => new ContaFinanceira
        {
            Id = Id,
            DataInsercao = DateTime.Now,
            Descricao = Descricao,
            Agencia = Agencia,
            DigitoAgencia = DigitoAgencia,
            Conta = Conta,
            DigitoConta = DigitoConta,
            Cpf = Cpf,
            Cnpj = Cnpj,
            Convenio = Convenio,
            Carteira = Carteira,
            CodigoTransmissao = CodigoTransmissao,
            ContaPadrao = ContaPadrao,
            Banco = Banco,
            Empresa = Empresa?.ToEntity(),
            ConvenioPagamento = ConvenioPagamento
        };

    }
}