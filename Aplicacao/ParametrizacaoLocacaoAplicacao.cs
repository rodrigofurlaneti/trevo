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
    public interface IParametrizacaoLocacaoAplicacao : IBaseAplicacao<ParametrizacaoLocacao>
    {
        void SalvarDados(ParametrizacaoLocacaoViewModel entity);
    }

    public class ParametrizacaoLocacaoAplicacao : BaseAplicacao<ParametrizacaoLocacao, IParametrizacaoLocacaoServico>, IParametrizacaoLocacaoAplicacao
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ITipoLocacaoAplicacao _tipoLocacaoAplicacao;

        public ParametrizacaoLocacaoAplicacao(IUnidadeAplicacao unidadeAplicacao,
                                               ITipoLocacaoAplicacao tipoLocacaoAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
            _tipoLocacaoAplicacao = tipoLocacaoAplicacao;
        }

        public void SalvarDados(ParametrizacaoLocacaoViewModel entity)
        {
            var entidade = new ParametrizacaoLocacao()
            {
                Id = entity.Id,
                DataInsercao = entity.DataInsercao,
                TipoLocacao = _tipoLocacaoAplicacao.BuscarPorId(entity.TipoLocacao.Id) ?? entity.TipoLocacao.ToEntity()//,
                                                                                                                       //Unidade = entity.ListaUnidades == null || entity.ListaUnidades.Count <= 0 ? entity.Unidade.ToEntity() : null
            };

            if (entidade.Id == 0)
            {

                foreach (var item in entity.ListaUnidades)
                {
                    if(this.BuscarPor(x => x.TipoLocacao.Id == entity.TipoLocacao.Id && x.Unidade.Id == item).Any())
                    {
                        var objUnidadeEncontrada = _unidadeAplicacao.BuscarPorId(item);
                        throw new BusinessRuleException("Tipo Locação " + entity.TipoLocacao.Descricao + " já possui cadastro para Unidade " + objUnidadeEncontrada.Nome);
                    }
                }

                foreach (var item in entity.ListaUnidades)
                {
                    entidade.Id = 0;
                    entidade.Unidade = _unidadeAplicacao.BuscarPorId(item);

                    if (!ObjetoValido(entidade))
                        throw new BusinessRuleException("Objeto Invalido!");

                    this.Salvar(entidade);
                }
            }
            else
            {
                var objNovo = this.BuscarPorId(entidade.Id) ?? entidade;
                objNovo.Id = entity.Id;
                objNovo.TipoLocacao = entidade.TipoLocacao;
                objNovo.Unidade = entity.Unidade != null ? _unidadeAplicacao.BuscarPorId(entity.Unidade.Id) : null;
                objNovo.DataInsercao = entidade.DataInsercao;

                if (!ObjetoValido(objNovo))
                    throw new BusinessRuleException("Objeto Invalido!");

                this.Salvar(objNovo);
            }
        }

        public bool ObjetoValido(ParametrizacaoLocacao entity)
        {
            if (entity.TipoLocacao == null || entity.TipoLocacao.Id == 0)
                throw new BusinessRuleException("Informe o Tipo de Locação!");

            if (entity.Unidade == null)
                throw new BusinessRuleException("Informe a Unidade");

            return true;
        }
    }
}