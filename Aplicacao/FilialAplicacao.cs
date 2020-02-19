using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IFilialAplicacao : IBaseAplicacao<Filial>
    {
    }

    public class FilialAplicacao : BaseAplicacao<Filial, IFilialServico>, IFilialAplicacao
    {
        private readonly IFilialServico _filialServico;
        private readonly ICidadeAplicacao _cidadeAplicacao;

        public FilialAplicacao(ICidadeAplicacao cidadeAplicacao, IFilialServico filialServico)
        {
            _cidadeAplicacao = cidadeAplicacao;
            _filialServico = filialServico;
        }

        public new void Salvar(Filial entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            var filialRetorno = BuscarPorId(entity.Id) ?? entity;
            filialRetorno.Id = entity.Id;
            filialRetorno.Descricao = entity.Descricao;
            filialRetorno.CNPJ = entity.CNPJ;
            filialRetorno.Endereco = entity.Endereco;
            filialRetorno.Contatos = entity.Contatos;
            filialRetorno.DataInsercao = entity.DataInsercao;
            filialRetorno.InscricaoEstadual = entity.InscricaoEstadual;
            filialRetorno.RazaoSocial = entity.RazaoSocial;
            filialRetorno.Empresa = entity.Empresa;
            filialRetorno.TipoFilial = entity.TipoFilial;
            filialRetorno.Endereco.Cidade = _cidadeAplicacao.BuscarPor(x => x.Descricao == entity.Endereco.Cidade.Descricao).FirstOrDefault() ?? entity.Endereco.Cidade;

            Servico.Salvar(filialRetorno);
        }

        public bool ObjetoValido(Filial entity)
        {
            if (string.IsNullOrEmpty(entity.CNPJ))
                throw new BusinessRuleException("Informe o CNPJ!");

            if (string.IsNullOrEmpty(entity.Descricao))
                throw new BusinessRuleException("Informe o Nome da Filial!");

            if (entity.Endereco == null)
                throw new BusinessRuleException("Informe o Endereco da Filial!");

            if (string.IsNullOrEmpty(entity.Endereco.Cep))
                throw new BusinessRuleException("Informe um CEP para cadastrar o endereço!");
                
            if (string.IsNullOrEmpty(entity.Endereco.Numero)
                || string.IsNullOrEmpty(entity.Endereco.Logradouro)
                || string.IsNullOrEmpty(entity.Endereco.Bairro)
                || entity.Endereco.Cidade?.Id > 0)
                throw new BusinessRuleException("Informe os dados do endereço!");

            return true;
        }

    }
}
