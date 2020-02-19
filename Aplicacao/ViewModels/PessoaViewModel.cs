using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
	public class PessoaViewModel
	{
		public int Id { get; set; }
		public DateTime DataInsercao { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public DateTime? DataNascimento { get; set; }
		public TrabalhoViewModel Trabalho { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public List<EnderecoViewModel> Enderecos { get; set; }
		public List<DocumentoViewModel> Documentos { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public List<ContatoViewModel> Contatos { get; set; }
		public IList<PessoaLojaViewModel> Lojas { get; set; }
        public IList<int> IdLojas { get; set; }
		public TipoPrograma Programa { get; set; }
		public int IdDocumentoCpf { get; set; }
		public string Cpf { get; set; }
		public string CpfNome => string.IsNullOrEmpty(Cpf) ? Nome : $"{Cpf} - {Nome}";
		public int IdDocumentoRg { get; set; }
		public string Rg { get; set; }
		public string OrgaoExpedidorRg { get; set; }
		public DateTime? DataExpedicaoRg { get; set; }
        public bool Ativo { get; set; }

        //Mapeamento para Importacao Apenas
        public EnderecoViewModel Endereco { get; set; }
		public ContatoViewModel Contato { get; set; }
		public ContatoViewModel ContatoCel { get; set; }
		public ContatoViewModel ContatoEmail { get; set; }
		public ContatoViewModel ContatoComercial { get; set; }
        
        //Pesquisa
        public string CPFPesquisa { get; set; }
		public string NomePesquisa { get; set; }


		public PessoaViewModel()
		{
			Enderecos = new List<EnderecoViewModel>();
			Contatos = new List<ContatoViewModel>();
            Documentos = new List<DocumentoViewModel>();
            IdLojas = new List<int>();

			Endereco = new EnderecoViewModel();
			Trabalho = new TrabalhoViewModel();

			Contato = new ContatoViewModel();
			ContatoCel = new ContatoViewModel();
			ContatoEmail = new ContatoViewModel();
			ContatoComercial = new ContatoViewModel();

            Ativo = true;
		}

		public PessoaViewModel(Pessoa pessoa)
		{
            Id = pessoa.Id;
            DataInsercao = pessoa.DataInsercao;
			Nome = pessoa.Nome;
			Sexo = pessoa.Sexo;
			DataNascimento = pessoa.DataNascimento;
			IdDocumentoCpf = pessoa?.Documentos?.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Cpf)?.Documento?.Id ?? 0;
			Cpf = pessoa?.Documentos?.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Cpf)?.Documento?.Numero ?? string.Empty;
			IdDocumentoRg = pessoa?.Documentos?.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Rg)?.Documento?.Id ?? 0;
			Rg = pessoa?.Documentos?.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Rg)?.Documento?.Numero ?? string.Empty;
			OrgaoExpedidorRg = pessoa?.Documentos?.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Rg)?.Documento?.OrgaoExpedidor ?? string.Empty;
			DataExpedicaoRg = pessoa?.Documentos?.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Rg)?.Documento?.DataExpedicao;
            Documentos = pessoa?.Documentos?.Select(x => new DocumentoViewModel(x.Documento)).ToList();
			Enderecos = pessoa?.Enderecos?.Select(x => new EnderecoViewModel(x.Endereco)).ToList();
			Contatos = pessoa?.Contatos?.Select(x => new ContatoViewModel(x.Contato))?.ToList();
			IdLojas = pessoa?.Lojas?.Select(x => x.Loja.Id).ToList();
			Trabalho = new TrabalhoViewModel(pessoa.Trabalho);
            Ativo = pessoa?.Ativo ?? false;

            var contato = pessoa?.Contatos?.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Residencial)?.Contato;
            Contato = contato != null ? new ContatoViewModel(contato) : new ContatoViewModel();

            var contatoCel = pessoa?.Contatos?.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Celular)?.Contato;
            ContatoCel = contatoCel != null ? new ContatoViewModel(contatoCel) : new ContatoViewModel();

            var contatoEmail = pessoa?.Contatos?.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Email)?.Contato;
            ContatoEmail = contatoEmail != null ? new ContatoViewModel(contatoEmail) : new ContatoViewModel();
        }

        public virtual Pessoa ToEntity() => new Pessoa
		{
			Id = this.Id,
			DataInsercao = DateTime.Now,
			Nome = this.Nome,
			Sexo = this.Sexo,
			DataNascimento = this.DataNascimento,
			Enderecos = this.RetornarEnderecos(),
			Contatos = this.RetornarContatos(),
			Documentos = this.TransformarDocumentos(),
			Lojas = IdLojas?.Select(x => new PessoaLoja { Loja = new Empresa { Id = x }, Pessoa = new Pessoa { Id = Id } }).ToList(),
			Trabalho = this.Trabalho?.ToEntity() ?? null,
            Ativo = this.Ativo
		};

		protected List<PessoaContato> RetornarContatos()
		{
			var contato = this?.Contatos ?? new List<ContatoViewModel>();

			if (this.Contato != new ContatoViewModel() && !string.IsNullOrEmpty(this.Contato.Numero) && !contato.Any(x => x.Numero == this.Contato.Numero))
			{
				this.Contato.Tipo = TipoContato.Residencial;
				contato.Add(this.Contato);
			}

            if (this.ContatoCel != new ContatoViewModel() && !string.IsNullOrEmpty(this.ContatoCel.Numero) && !contato.Any(x => x.Numero == this.ContatoCel.Numero))
            {
                this.ContatoCel.Tipo = TipoContato.Celular;
                contato.Add(this.ContatoCel);
            }

            if (this.ContatoComercial != null && this.ContatoComercial != new ContatoViewModel() && !string.IsNullOrEmpty(this.ContatoComercial.Numero) && !contato.Any(x => x.Numero == this.ContatoComercial.Numero))
            {
                this.ContatoComercial.Tipo = TipoContato.Comercial;
                contato.Add(this.ContatoComercial);
            }
            
			if (this.ContatoEmail != new ContatoViewModel() && !string.IsNullOrEmpty(this.ContatoEmail.Email) && !contato.Any(x => !string.IsNullOrEmpty(x.Email) && x.Email == this.ContatoEmail.Email))
			{
				this.ContatoEmail.Tipo = TipoContato.Email;
				contato.Add(this.ContatoEmail);
			}

            return contato.Select(x => new PessoaContato { Contato = x.ToEntity() }).ToList();
		}

		protected List<PessoaEndereco> RetornarEnderecos()
		{
			var endereco = this?.Enderecos ?? new List<EnderecoViewModel>();

			if (endereco != null && this.Endereco != null && !string.IsNullOrEmpty(this.Endereco.Logradouro) && !endereco.Any(x => x.Logradouro == this.Endereco.Logradouro))
				endereco.Add(this.Endereco);

			var _retorno = new List<PessoaEndereco>();
            if (endereco != null)
                foreach (var item in endereco)
                    if (!string.IsNullOrEmpty(item.Logradouro))
                        _retorno.Add(new PessoaEndereco { Endereco = item.ToEntity() });

            return _retorno;
		}

		private List<PessoaDocumento> TransformarDocumentos()
		{
            var listaDocumentos = this.Documentos?.Select(x => x.ToEntity()).ToList() ?? new List<Documento>();

            if (!string.IsNullOrEmpty(this.Cpf))
            {
                var cpf = new Documento(TipoDocumento.Cpf, this.Cpf, DateTime.Now);

                if (!listaDocumentos.Any(x => x.Numero == cpf.Numero))
                    listaDocumentos.Add(cpf); 
            }

            if (!string.IsNullOrEmpty(this.Rg)) {
                var rg = new Documento(TipoDocumento.Rg, this.Rg, DateTime.Now);

                if (!listaDocumentos.Any(x => x.Numero == rg.Numero))
                    listaDocumentos.Add(rg);
            }

			return listaDocumentos.Select(x => new PessoaDocumento { Documento = x }).ToList();
		}

        private List<Documento> RecoverDocument()
        {
            var documents = new List<Documento>();
            if (!string.IsNullOrEmpty(Cpf))
                documents.Add(new Documento(TipoDocumento.Cpf, Cpf, DateTime.Now, Id = IdDocumentoCpf));
            if (!string.IsNullOrEmpty(Rg))
                documents.Add(new Documento(TipoDocumento.Rg, Rg, DateTime.Now, IdDocumentoRg, OrgaoExpedidorRg, null, DataExpedicaoRg));

            return documents;
        }

        public int Idade
        {
            get
            {
                if (!this.DataNascimento.HasValue || (this.DataNascimento < System.Data.SqlTypes.SqlDateTime.MinValue.Value))
                    return 0;

                var idade = DateTime.Today.Year - this.DataNascimento.Value.Year;
                if (this.DataNascimento > DateTime.Today.AddYears(-idade))
                    idade--;

                return idade;
            }
        }

        public string DocumentoComFormatacao()
        {

            if (string.IsNullOrEmpty(Cpf)) return "";

            var doc = Cpf.Replace("-", "").Replace(".", "").Replace("/", "");

            if (doc.Length <= 11)
            {
                var onzeDigitosCpf = doc.PadLeft(11, '0');
                var digitos = Convert.ToInt64(Regex.Replace(onzeDigitosCpf, "[^0-9]", ""));
                return digitos.ToString(@"000\.000\.000\-00");
            }
            else
            {
                var onzeDigitosCpf = doc.PadLeft(14, '0');
                var digitos = Convert.ToInt64(Regex.Replace(onzeDigitosCpf, "[^0-9]", ""));
                return digitos.ToString(@"00\.000\.000/0000-00");
            }
        }
    }
}