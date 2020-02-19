using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class ClienteViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public PessoaViewModel Pessoa { get; set; }
        public string DocumentoCpf => Pessoa?.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Cpf)?.Numero ?? string.Empty;
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public IList<ClienteVeiculoViewModel> Veiculos { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }

        public string NomeExibicao
        {
            get
            {
                return string.IsNullOrEmpty(NomeFantasia)
                        ? string.IsNullOrEmpty(RazaoSocial)
                            ? string.IsNullOrEmpty(Pessoa?.Nome)
                                ? string.Empty
                                : Pessoa?.Nome
                            : RazaoSocial
                        : NomeFantasia;
            }
        }
        public string DocumentoPessoal
        {
            get
            {
                return string.IsNullOrEmpty(Cnpj)
                        ? string.IsNullOrEmpty(Cpf)
                            ? string.Empty
                            : Cpf.FormatCpfCnpj()
                        : Cnpj.FormatCpfCnpj();
            }
        }

        public string Cnpj { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public TipoPessoa TipoPessoa { get; set; }

        public string NumeroContrato { get; set; }

        public bool ExigeNotaFiscal { get; set; }

        public bool NotaFiscalSemDesconto { get; set; }

        public string NomeConvenio { get; set; }
        [AllowHtml]
        public string Observacao { get; set; }

        public IList<ClienteUnidadeViewModel> Unidades { get; set; }

        private List<int> ListaIds { get; set; }
        public List<int> ListaUnidade
        {
            get
            {

                var retorno = new List<int>();

                if (Unidades != null)
                    retorno.AddRange(Unidades.Select(ClienteUnidade => ClienteUnidade.Unidade.Id));

                return ListaIds != null && ListaIds.Any()
                        ? ListaIds
                        : retorno;
            }
            set { ListaIds = value; }
        }

        //Controle de Tela
        public string Cpf { get; set; }

        public string Descricao
        {
            get
            {
                if (PessoaFisica)
                    return Pessoa?.Nome ?? string.Empty;
                else if (PessoaJuridica)
                    return NomeFantasia ?? string.Empty;

                return string.Empty;
            }
        }

        public string DescricaoDocumento
        {
            get
            {
                if (PessoaFisica)
                    return Pessoa?.Cpf ?? string.Empty;
                else if (PessoaJuridica)
                    return Cnpj ?? string.Empty;

                return string.Empty;
            }
        }

        public ContaCorrenteClienteViewModel ContaCorrenteCliente { get; set; }

        public OcorrenciaClienteViewModel OcorrenciaCliente { get; set; }

        public SeloClienteViewModel SeloCliente { get; set; }

        public ClienteViewModel()
        {
            Pessoa = new PessoaViewModel();
            SeloCliente = new SeloClienteViewModel();
        }

        public ClienteViewModel(Cliente cliente)
        {
            if (cliente != null)
            {
                Id = cliente.Id;
                DataInsercao = cliente?.Pessoa?.DataInsercao ?? DateTime.Now;
                Pessoa = new PessoaViewModel(cliente?.Pessoa ?? new Pessoa());
                Veiculos = new ClienteVeiculoViewModel().ListaVeiculos(cliente?.Veiculos);
                NomeFantasia = cliente?.NomeFantasia;
                RazaoSocial = cliente?.RazaoSocial;
                TipoPessoa = cliente.TipoPessoa;
                Cnpj = cliente?.Pessoa?.DocumentoCnpj;
                Cpf = cliente?.Pessoa?.DocumentoCpf;
                InscricaoEstadual = cliente?.Pessoa?.DocumentoIe;
                InscricaoMunicipal = cliente?.Pessoa?.DocumentoIm;
                TipoPessoa = cliente.TipoPessoa;
                ListaUnidade = cliente.ListaUnidade;
                ExigeNotaFiscal = cliente.ExigeNotaFiscal;
                NomeConvenio = cliente.NomeConvenio;
                Observacao = cliente.Observacao;
                NotaFiscalSemDesconto = cliente.NotaFiscalSemDesconto;
                ContaCorrenteCliente = AutoMapper.Mapper.Map<ContaCorrenteClienteViewModel>(cliente.ContaCorrenteCliente);
                SeloCliente = AutoMapper.Mapper.Map<SeloClienteViewModel>(cliente.SeloCliente);
            }
        }

        public Cliente ToEntity()
        {
            var cliente = new Cliente();

            cliente.Id = Id;
            cliente.DataInsercao = DateTime.Now;
            cliente.Pessoa = Pessoa?.ToEntity();
            cliente.Veiculos = new ClienteVeiculoViewModel().ListaVeiculos(Veiculos);
            cliente.NomeFantasia = this.NomeFantasia;
            cliente.RazaoSocial = this.RazaoSocial;
            cliente.Pessoa.DocumentoCpf = this.Pessoa.Cpf;
            cliente.Pessoa.DocumentoRg = this.Pessoa.Rg;
            cliente.Pessoa.DocumentoCnpj = this.Cnpj;
            cliente.Pessoa.DocumentoIe = this.InscricaoEstadual;
            cliente.Pessoa.DocumentoIm = this.InscricaoMunicipal;
            cliente.TipoPessoa = this.TipoPessoa;
            cliente.ListaUnidade = this.ListaUnidade;
            cliente.ExigeNotaFiscal = this.ExigeNotaFiscal;
            cliente.NomeConvenio = this.NomeConvenio;
            cliente.Observacao = this.Observacao;
            cliente.NotaFiscalSemDesconto = this.NotaFiscalSemDesconto;
            cliente.ContaCorrenteCliente = ContaCorrenteCliente?.ContaCorrenteClienteId > 0 || ContaCorrenteCliente?.ContaCorrenteClienteDetalhes?.Count > 0 ?
                                                    AutoMapper.Mapper.Map<ContaCorrenteCliente>(ContaCorrenteCliente) : null;
            cliente.SeloCliente = SeloCliente == null ? new SeloCliente() : SeloCliente.ToEntity();
            
            return cliente;
        }

        private bool PessoaFisica => TipoPessoa.Fisica == TipoPessoa;
        private bool PessoaJuridica => TipoPessoa.Juridica == TipoPessoa;


    }
}