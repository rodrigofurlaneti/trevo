using System;
using System.Collections.Generic;
using System.Linq;
using Core.Exceptions;
using Core.Validators;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System.Data.SqlTypes;

namespace Dominio
{
    public interface IPessoaServico : IBaseServico<Pessoa>
    {
        void ValidaPessoa(Pessoa entity);
        Pessoa Salvar(Pessoa entity, bool validar = true);
        Pessoa Update(Pessoa entity, bool validar);
        Pessoa BuscarPorCpfComOuSemMascara(string cpf);
        List<Pessoa> PesquisarComFiltro(string nome, string CPF);
        List<Pessoa> PesquisarComFiltroNaoDevedores(string nome, string CPF);
	    void BuscarAdicionarCidade(Endereco endereco);
	    void BuscarAdicionarCidade(List<Endereco> enderecos);
	}

    public class PessoaServico : BaseServico<Pessoa, IPessoaRepositorio>, IPessoaServico
    {
        #region Private Properties
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly ICidadeServico _cidadeServico;
        private readonly IContatoServico _contatoServico;
        private readonly IDocumentoServico _documentoServico;
        private readonly IEmpresaServico _lojaServico;
        private readonly IEnderecoServico _enderecoServico;
	    private readonly IEstadoServico _estadoServico;

	    #endregion

        #region Constructor
        public PessoaServico(IPessoaRepositorio pessoaRepositorio, ICidadeServico cidadeServico, IContatoServico contatoServico, IDocumentoServico documentoServico, IEmpresaServico lojaServico, IEnderecoServico enderecoServico, IEstadoServico estadoServico)
        {
            _pessoaRepositorio = pessoaRepositorio;
            _cidadeServico = cidadeServico;
            _contatoServico = contatoServico;
            _documentoServico = documentoServico;
            _lojaServico = lojaServico;
            _enderecoServico = enderecoServico;
	        _estadoServico = estadoServico;
        }
        #endregion

        public void ValidaPessoa(Pessoa entity)
        {
            if (string.IsNullOrEmpty(entity.Nome))
                throw new BusinessRuleException("Informe o Nome!");
            if (string.IsNullOrEmpty(entity.Sexo))
                throw new BusinessRuleException("Informe o Gênero!");
            if (entity.DataNascimento <= DateTime.MinValue
                || entity.DataNascimento >= DateTime.MaxValue
                || (entity.DataNascimento.HasValue && (entity.DataNascimento.Value.Date >= DateTime.Now.Date)))
                throw new BusinessRuleException("Informe uma data de nascimento válida!");

            if (entity.Enderecos == null)
                throw new BusinessRuleException("Informe o Endereco do Cadastro!");
            if (string.IsNullOrEmpty(entity.Enderecos.FirstOrDefault().Endereco.Cep))
                throw new BusinessRuleException("Informe um CEP para cadastrar o endereço!");
            if (string.IsNullOrEmpty(entity.Enderecos.FirstOrDefault().Endereco.Numero)
                || string.IsNullOrEmpty(entity.Enderecos.FirstOrDefault().Endereco.Logradouro)
                || string.IsNullOrEmpty(entity.Enderecos.FirstOrDefault().Endereco.Bairro)
                || entity.Enderecos.FirstOrDefault().Endereco.Cidade?.Id > 0)
                throw new BusinessRuleException("Informe os dados do endereço!");

            if (entity.Documentos == null
                || !entity.Documentos.Any()
                || entity.Documentos.All(x => x.Documento.Tipo != TipoDocumento.Cpf)
                || string.IsNullOrEmpty(entity.Documentos.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Cpf)?.Documento?.Numero)
                || !string.IsNullOrEmpty(entity.Documentos.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Cpf)?.Documento?.Numero)
                    && !Validators.IsCpf(entity.Documentos.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Cpf)?.Documento?.Numero))
                throw new BusinessRuleException("Informe um CPF Válido!");
        }

	    public void BuscarAdicionarCidade(Endereco endereco)
	    {
		    BuscarAdicionarCidade(new List<Endereco> { endereco });
	    }
		
	    public void BuscarAdicionarCidade(List<Endereco> enderecos)
	    {
		    if (enderecos?.Count > 0)
		    {
			    foreach (var item in enderecos)
			    {
				    if (item.Cidade != null)
				    {
					    var cidade = _cidadeServico.BuscarPor(c => c.Descricao == item.Cidade.Descricao && c.Estado.Sigla == item.Cidade.Estado.Sigla).FirstOrDefault();

					    if (cidade == null)
					    {
						    var estado = _estadoServico.BuscarPor(e => e.Sigla == item.Cidade.Estado.Sigla).FirstOrDefault();
						    if (estado != null)
						    {
							    cidade = new Cidade
							    {
								    Descricao = item.Cidade.Descricao,
								    Estado = estado
							    };

							    _cidadeServico.Salvar(cidade);
						    }
					    }

					    item.Cidade = cidade;
				    }
			    }
		    }
	    }
		public Pessoa Salvar(Pessoa entity, bool validarPessoa = true)
        {
            if (validarPessoa)
                ValidaPessoa(entity);

            var itemSalvar = BuscarPorId(entity.Id) ??
                             BuscarPor(x => x.Documentos.Any(d => d.Documento.Numero == entity.DocumentoCpf && d.Documento.Tipo == TipoDocumento.Cpf)).FirstOrDefault() ??
                             new Pessoa();

			var listaEnderecosRemover = new List<int>();
            var listaDocumentosRemover = new List<int>();
            var listaContatosRemover = new List<int>();

            //CAMPOS COMUNS
            itemSalvar.Id = itemSalvar.Id > 0 ? itemSalvar.Id : entity.Id;
            itemSalvar.DataInsercao = entity.DataInsercao;
            itemSalvar.Nome = entity.Nome;
            itemSalvar.Sexo = entity.Sexo;
            itemSalvar.DataNascimento = entity.DataNascimento;
            itemSalvar.Trabalho = string.IsNullOrEmpty(entity?.Trabalho?.Empresa) ? null : entity.Trabalho;
	        itemSalvar.Lojas = entity.Lojas;
            itemSalvar.Ativo = entity.Ativo;

            listaEnderecosRemover = itemSalvar.Enderecos?.Where(x => !entity?.Enderecos?.Any(p => p.Endereco.Id == x.Endereco.Id) ?? false)?.Select(x=> x.Endereco.Id)?.ToList() ?? new List<int>();
            listaDocumentosRemover = itemSalvar.Documentos?.Where(x => !entity?.Documentos?.Any(p => p.Documento.Id == x.Documento.Id) ?? false)?.Select(x=> x.Documento.Id)?.ToList() ?? new List<int>();
            listaContatosRemover = itemSalvar.Contatos?.Where(x => !entity?.Contatos?.Any(p => p.Contato.Id == x.Contato.Id) ?? false)?.Select(x=> x.Contato.Id)?.ToList() ?? new List<int>();
            
            itemSalvar.Enderecos = entity.Enderecos?.Where(x => !string.IsNullOrEmpty(x.Endereco.Logradouro))?.ToList() ?? null;
            itemSalvar.Enderecos?.ToList()?.ForEach(x => { x.Endereco.Cidade = x.Endereco.Cidade == null ? null : _cidadeServico.BuscarPor(endereco => endereco.Descricao == x.Endereco.Cidade.Descricao).FirstOrDefault(); });
            itemSalvar.Documentos = entity.Documentos?.Where(x => x.Documento != null && !string.IsNullOrEmpty(x.Documento.Numero))?.ToList();
            itemSalvar.Contatos = entity.Contatos?.Where(x => !string.IsNullOrEmpty(x.Contato.Email) || !string.IsNullOrEmpty(x.Contato.Numero))?.ToList();

            Repositorio.Save(itemSalvar);

            //Remover Enderecos Inuteis
            listaEnderecosRemover.ForEach(x => _enderecoServico.ExcluirPorId(x));

            //Remover Documentos Inuteis
            listaDocumentosRemover.ForEach(x => _documentoServico.ExcluirPorId(x));

            //Remover Contatos Inuteis
            listaContatosRemover.ForEach(x => _contatoServico.ExcluirPorId(x));

            return itemSalvar;
        }

        public Pessoa Update(Pessoa entity, bool validar = true)
        {
            if (validar)
                ValidaPessoa(entity);

            var itemSalvar = BuscarPorId(entity.Id) ??
                             BuscarPor(x => x.Documentos.Any(d => d.Documento.Numero == entity.DocumentoCpf && d.Documento.Tipo == TipoDocumento.Cpf)).FirstOrDefault() ??
                             new Pessoa();

            if (itemSalvar.Id <= 0 && string.IsNullOrEmpty(entity.Nome)) return itemSalvar;

            //CAMPOS COMUNS
            itemSalvar.Id = itemSalvar.Id > 0 ? itemSalvar.Id : entity.Id;
            itemSalvar.DataInsercao = DateTime.Now;
            itemSalvar.Nome = entity.Nome ?? itemSalvar.Nome;
            itemSalvar.Sexo = entity.Sexo ?? itemSalvar.Sexo;
            itemSalvar.DataNascimento = entity.DataNascimento > SqlDateTime.MinValue.Value ? entity.DataNascimento : itemSalvar.DataNascimento;
            itemSalvar.Trabalho = !string.IsNullOrEmpty(entity?.Trabalho?.Empresa) ? entity.Trabalho : !string.IsNullOrEmpty(itemSalvar.Trabalho?.Empresa) ? itemSalvar.Trabalho : null;
            itemSalvar.Ativo = entity.Ativo;

            //ENDERECOS
            foreach (var item in entity.Enderecos)
            {
                if (!itemSalvar.Enderecos.Any(x => x.Endereco?.Logradouro == item.Endereco.Logradouro &&
                x.Endereco.Numero == item.Endereco.Numero &&
                x.Endereco.Bairro == item.Endereco.Bairro))
                    if (!string.IsNullOrEmpty(item.Endereco.Logradouro))
                    {
                        item.Endereco.Cidade = !string.IsNullOrEmpty(item.Endereco?.Cidade?.Descricao) ? _cidadeServico.BuscarPor(endereco => endereco.Descricao == item.Endereco.Cidade.Descricao).FirstOrDefault() : null;
                        itemSalvar.Enderecos.Add(item);
                    }
            }

            //DOCUMENTOS
            foreach (var item in entity.Documentos)
            {
                if (!itemSalvar.Documentos.Any(x => x.Documento?.Numero == item.Documento.Numero &&
                x.Documento.Tipo == item.Documento.Tipo))
                    if (!string.IsNullOrEmpty(item.Documento.Numero))
                        itemSalvar.Documentos.Add(item);
            }

            //CONTATOS
            foreach (var item in entity.Contatos)
            {
                if (!itemSalvar.Contatos.Any(x => (x.Contato?.DDD == item.Contato.DDD && x.Contato?.Numero == item.Contato.Numero && x.Contato.Tipo == item.Contato.Tipo) ||
                !string.IsNullOrEmpty(x.Contato?.Email) && !string.IsNullOrEmpty(item.Contato.Email) && x.Contato.Email == item.Contato.Email))
                    if (!string.IsNullOrEmpty(item.Contato.Numero) || !string.IsNullOrEmpty(item.Contato.Email))
                        itemSalvar.Contatos.Add(item);
            }

            Repositorio.Save(itemSalvar);

            return itemSalvar;
        }

        public List<Pessoa> PesquisarComFiltro(string nome, string CPF)
        {
            return _pessoaRepositorio.PesquisarComFiltro(nome, CPF);
        }

        public Pessoa BuscarPorCpfComOuSemMascara(string cpf)
        {
            return _pessoaRepositorio.PesquisarComFiltro(string.Empty, cpf)?.FirstOrDefault() ?? null;
        }

        public List<Pessoa> PesquisarComFiltroNaoDevedores(string nome, string CPF)
        {
            return _pessoaRepositorio.PesquisarComFiltroNaoDevedores(nome, CPF);
        }
    }
}