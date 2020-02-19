using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Extensions;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a Cliente
    /// </summary>
    public class CondutorSoftparkViewModel : BaseSoftparkViewModel
    {
        public string Cnh { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Nome { get; set; }
        public bool InSenha { get; set; }
        public string Senha { get; set; }
        public int ClienteId { get; set; }
        public bool InRestrito { get; set; }
        public bool InVagas { get; set; }
        public DateTime DataGravacao { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

        public int? ERPId { get; set; }

        public virtual IList<ContratoSoftparkViewModel> Contratos { get; set; }

        public virtual IList<ContratoCondominoSoftparkViewModel> ContratosCondomino { get; set; }

        public CondutorSoftparkViewModel()
        {
        }

        public CondutorSoftparkViewModel(Cliente cliente, IList<ClienteCondomino> contratosCondomino, IList<ContratoMensalista> contratos)
        {
            Id = cliente.Id;

            var endereco = cliente.Pessoa.Enderecos?.FirstOrDefault()?.Endereco;

            DataInsercao = cliente.DataInsercao;
            Cnh = cliente.Pessoa.DocumentoCnh ?? string.Empty;
            Cpf = cliente.Pessoa.DocumentoCpf?.UnformatCpfCnpj() ?? string.Empty;
            Rg = cliente.Pessoa.DocumentoRg ?? string.Empty;
            Nome = cliente.Pessoa.Nome;
            ClienteId = cliente.Id;
            ERPId = cliente.Id;
            InVagas = true;
            DataGravacao = cliente.DataInsercao;
            Cep = endereco?.Cep?.RemoveSpecialCharacters() ?? string.Empty;
            Endereco = endereco?.Logradouro ?? string.Empty;
            Bairro = endereco?.Bairro ?? string.Empty;
            Cidade = endereco?.Cidade?.Descricao ?? string.Empty;
            Uf = endereco?.Cidade?.Estado?.Sigla ?? string.Empty;
            Contratos = contratos != null && contratos.Any() ? contratos?.Select(x => new ContratoSoftparkViewModel(x))?.ToList() : null;
            ContratosCondomino = contratosCondomino != null && contratosCondomino.Any() ? contratosCondomino?.Select(x => new ContratoCondominoSoftparkViewModel(x))?.ToList() : null;
        }

        public Cliente ToCliente(Cidade cidade)
        {
            return new Cliente
            {
                Id = 0,
                DataInsercao = this.DataInsercao,
                Pessoa = this.ToPessoa(cidade),
                TipoPessoa = TipoPessoa.Fisica,
                IdSoftpark = this.Id > 0 ? this.Id : default(int?),
                Veiculos = Contratos.SelectMany(x => x.Carros).Select(y => y.Carro.ToClienteVeiculo()).ToList()
            };
        }

        public Pessoa ToPessoa(Cidade cidade)
        {
            var pessoa = new Pessoa
            {
                Id = 0,
                DataInsercao = this.DataInsercao,
                Nome = this.Nome,
            };

            var documentoCpf = !string.IsNullOrEmpty(this.Cpf) ? this.ToPessoaDocumento(TipoDocumento.Cpf, this.Cpf) : null;
            var documentoRg = !string.IsNullOrEmpty(this.Rg) ? this.ToPessoaDocumento(TipoDocumento.Rg, this.Rg) : null;
            var documentoCnh = !string.IsNullOrEmpty(this.Cnh) ? this.ToPessoaDocumento(TipoDocumento.Cnh, this.Cnh) : null;

            pessoa.Documentos = new List<PessoaDocumento>();

            if (documentoCpf != null)
                pessoa.Documentos.Add(documentoCpf);

            if (documentoRg != null)
                pessoa.Documentos.Add(documentoRg);

            if (documentoCnh != null)
                pessoa.Documentos.Add(documentoCnh);

            pessoa.Enderecos = new List<PessoaEndereco> { this.ToPessoaEndereco(pessoa.Id, cidade) };

            return pessoa;
        }

        public PessoaDocumento ToPessoaDocumento(TipoDocumento tipoDocumento, string numero)
        {
            return new PessoaDocumento
            {
                Id = 0,
                DataInsercao = DateTime.Now,
                Tipo = tipoDocumento,
                Documento = new Documento(tipoDocumento, numero, DateTime.Now)
            };
        }

        public PessoaEndereco ToPessoaEndereco(int pessoaId, Cidade cidade)
        {
            return new PessoaEndereco
            {
                Id = 0,
                DataInsercao = DateTime.Now,
                Pessoa = pessoaId,
                Endereco = new Endereco
                {
                    Id = 0,
                    Cep = this.Cep,
                    Logradouro = this.Endereco,
                    Complemento =
                    Bairro = this.Bairro,
                    DataInsercao = DateTime.Now,
                    Cidade = cidade
                }
            };
        }

        public List<ContratoMensalista> ToContratos(Cliente cliente, List<Veiculo> veiculos)
        {
            var contratosMensalista = new List<ContratoMensalista>();

            foreach (var contrato in this.Contratos)
            {
                contratosMensalista.Add(contrato.ToContratoMensalista(cliente, veiculos));
            }

            return contratosMensalista;
        }

        public List<ClienteUnidade> ToClienteUnidades(List<Unidade> unidades)
        {
            var clienteUnidades = new List<ClienteUnidade>();

            foreach (var condutorEstacionamento in this.Contratos.Select(x => x.Estacionamento))
            {
                clienteUnidades.Add(new ClienteUnidade
                {
                    Id = 0,
                    DataInsercao = DataInsercao,
                    Unidade = unidades.FirstOrDefault(x => x.Id == condutorEstacionamento.Id)
                });
            }

            return clienteUnidades;
        }

        public CondutorSoftparkViewModel Clone()
        {
            return (CondutorSoftparkViewModel)this.MemberwiseClone();
        }
    }
}
