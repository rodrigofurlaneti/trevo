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
    public interface IEstoqueAplicacao : IBaseAplicacao<Estoque>
    {
        void SalvarDados(EstoqueViewModel entity);
    }

    public class EstoqueAplicacao : BaseAplicacao<Estoque, IEstoqueServico>, IEstoqueAplicacao
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public EstoqueAplicacao(IUnidadeAplicacao unidadeAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
        }

        public void SalvarDados(EstoqueViewModel entity)
        {
            if (!ObjetoValido(entity.ToEntity()))
                throw new BusinessRuleException("Objeto Invalido!");

            var estoque = BuscarPorId(entity.Id) ?? entity.ToEntity();
            estoque.Id = entity.Id;
            estoque.Nome = entity.Nome;
            estoque.DataInsercao = entity.DataInsercao;
            estoque.Numero = entity.Numero;
            estoque.Tipo = entity.Tipo;
            estoque.CidadeNome = entity.CidadeNome;
            estoque.UF = entity.UF;
            estoque.Logradouro = entity.Logradouro;
            estoque.EstoquePrincipal = entity.EstoquePrincipal;

            if (entity == null || entity.Unidade.Id == 0)
                estoque.Unidade = null;
            else
                estoque.Unidade = _unidadeAplicacao.BuscarPorId(entity.Unidade.Id);

            if (estoque.EstoquePrincipal)
                SubstituirEstoquePrincipal();

            Servico.Salvar(estoque);
        }

        public void SubstituirEstoquePrincipal()
        {
            var estoque = Servico.PrimeiroPor(x => x.EstoquePrincipal == true);
            if (estoque != null)
            {
                estoque.EstoquePrincipal = false;
                Servico.Salvar(estoque);
            }
        }

        public bool ObjetoValido(Estoque entity)
        {
            if (string.IsNullOrEmpty(entity.Nome))
                throw new BusinessRuleException("Informe o Nome!");

            if (string.IsNullOrEmpty(entity.Cep))
                throw new BusinessRuleException("Informe o Cep!");

            if (string.IsNullOrEmpty(entity.UF))
                throw new BusinessRuleException("Informe o Estado!");

            if (string.IsNullOrEmpty(entity.CidadeNome))
                throw new BusinessRuleException("Informe a Cidade!");

            if (string.IsNullOrEmpty(entity.Bairro))
                throw new BusinessRuleException("Informe o Bairro!");

            if (string.IsNullOrEmpty(entity.Logradouro))
                throw new BusinessRuleException("Informe o Endereço!");

            if (string.IsNullOrEmpty(entity.Numero))
                throw new BusinessRuleException("Informe o Numero!");

            if (entity.Tipo != TipoEndereco.Comercial && entity.Tipo != TipoEndereco.Residencial)
                throw new BusinessRuleException("Informe o Tipo!");

            return true;
        }
    }
}