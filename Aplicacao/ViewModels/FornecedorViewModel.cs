using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aplicacao.ViewModels
{
    public class FornecedorViewModel : PessoaViewModel
    {
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public bool ReceberCotacaoPorEmail { get; set; }

        public BancoViewModel Banco { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string CPFCNPJ { get; set; }
        public string Beneficiario { get; set; }

        public FornecedorViewModel()
        {
            this.Endereco = new EnderecoViewModel();
            this.Contatos = new List<ContatoViewModel>();
        }

        public FornecedorViewModel(Fornecedor entidade)
        {
            this.Id = entidade?.Id ?? 0;
            this.DataInsercao = entidade?.DataInsercao ?? DateTime.Now;
            this.Nome = entidade?.Nome;
            this.NomeFantasia = entidade?.NomeFantasia;
            this.RazaoSocial = entidade?.RazaoSocial;
            this.TipoPessoa = entidade.TipoPessoa;
            this.Cpf = entidade.DocumentoCpf;
            this.Rg = entidade.DocumentoRg;
            this.Cnpj = entidade.DocumentoCnpj;
            this.InscricaoEstadual = entidade.DocumentoIe;
            this.InscricaoMunicipal = entidade.DocumentoIm;
            this.Endereco = new EnderecoViewModel(entidade.Enderecos?.FirstOrDefault()?.Endereco);
            this.Contatos = entidade.Contatos?.Select(x => new ContatoViewModel(x.Contato))?.ToList();
            this.ReceberCotacaoPorEmail = entidade.ReceberCotacaoPorEmail;
            this.Banco = entidade?.Banco != null ? new BancoViewModel(entidade.Banco) : new BancoViewModel();
            this.Agencia = entidade.Agencia;
            this.DigitoAgencia = entidade.DigitoAgencia;
            this.Conta = entidade.Conta;
            this.DigitoConta = entidade.DigitoConta;
            this.CPFCNPJ = entidade.CPFCNPJ;
            this.Beneficiario = entidade.Beneficiario;
        }

        public Fornecedor ViewModelToEntity()
        {
            return new Fornecedor()
            {
                Id = this.Id,
                Nome = Nome,
                DocumentoCpf = this.Cpf,
                DocumentoRg = this.Rg,
                DocumentoCnpj = this.Cnpj,
                DocumentoIe = this.InscricaoEstadual,
                DocumentoIm = this.InscricaoMunicipal,
                Enderecos = this.RetornarEnderecos(),
                Contatos = this.RetornarContatos(),
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
                NomeFantasia = this?.NomeFantasia,
                RazaoSocial = this?.RazaoSocial,
                TipoPessoa = this.TipoPessoa,
                ReceberCotacaoPorEmail = this.ReceberCotacaoPorEmail,
                Banco = this.Banco?.ToEntity(),
                Agencia = this.Agencia,
                DigitoAgencia = this.DigitoAgencia,
                Conta = this.Conta,
                DigitoConta = this.DigitoConta,
                CPFCNPJ = this.CPFCNPJ,
                Beneficiario = this.Beneficiario,
            };
        }
    }
}
