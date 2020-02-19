using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;

namespace Aplicacao
{

    public interface IContaContabilAplicacao : IBaseAplicacao<ContaContabil>
    {
        IList<ContaContabil> BuscarDadosSimples();
    }

    public class ContaContabilAplicacao : BaseAplicacao<ContaContabil, IContaContabilServico>, IContaContabilAplicacao
    {
        private readonly ICidadeAplicacao _cidadeAplicacao;
        private readonly IContaContabilServico _contaContabilServico;

        public ContaContabilAplicacao(IContaContabilServico contaContabilServico)
        {
            _contaContabilServico = contaContabilServico;
        }

        public new void Salvar(ContaContabil entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            var ContaContabil = BuscarPorId(entity.Id) ?? entity;
            ContaContabil.Id = entity.Id;
            ContaContabil.Descricao = entity.Descricao;
            ContaContabil.DataInsercao = entity.DataInsercao;
            ContaContabil.Ativo = entity.Ativo;
            ContaContabil.Fixa = entity.Fixa;

            Servico.Salvar(ContaContabil);
        }

        public bool ObjetoValido(ContaContabil entity)
        {
            if (string.IsNullOrEmpty(entity.Descricao))
                throw new BusinessRuleException("Informe a descrição!");

            return true;
        }

        public IList<ContaContabil> BuscarDadosSimples()
        {
            return _contaContabilServico.BuscarDadosSimples();
        }
    }
}