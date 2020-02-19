using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;

namespace Aplicacao
{
    public interface IEmpresaAplicacao : IBaseAplicacao<Empresa>
    { }

    public class EmpresaAplicacao : BaseAplicacao<Empresa, IEmpresaServico>, IEmpresaAplicacao
    {
        private readonly ICidadeAplicacao _cidadeAplicacao;
	    private readonly IPessoaServico _pessoaServico;

	    public EmpresaAplicacao(ICidadeAplicacao cidadeAplicacao, IPessoaServico pessoaServico)
	    {
		    _cidadeAplicacao = cidadeAplicacao;
		    _pessoaServico = pessoaServico;
	    }

        public new void Salvar(Empresa entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");


	        _pessoaServico.BuscarAdicionarCidade(entity.Endereco);

			Servico.Salvar(entity);
        }

        public bool ObjetoValido(Empresa entity)
        {
			if (string.IsNullOrEmpty(entity.CNPJ))
		        throw new BusinessRuleException("Informe o CNPJ!");

	        if (string.IsNullOrEmpty(entity.Descricao))
		        throw new BusinessRuleException("Informe o Nome da Empresa!");

	        if (entity.Endereco == null)
		        throw new BusinessRuleException("Informe o Endereco da Empresa!");
	        if (string.IsNullOrEmpty(entity.Endereco.Cep))
		        throw new BusinessRuleException("Informe um CEP para cadastrar o endereço!");
	        if (string.IsNullOrEmpty(entity.Endereco.Numero)
	            || string.IsNullOrEmpty(entity.Endereco.Logradouro)
	            || string.IsNullOrEmpty(entity.Endereco.Bairro))
		        throw new BusinessRuleException("Informe os dados do endereço!");

	        return true;
		}
    }
}