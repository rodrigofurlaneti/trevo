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
    public interface ITipoEquipeAplicacao : IBaseAplicacao<TipoEquipe>
    { }

    public class TipoEquipeAplicacao : BaseAplicacao<TipoEquipe, ITipoEquipeServico>, ITipoEquipeAplicacao
    {
        private readonly ICidadeAplicacao _cidadeAplicacao;

        public new void Salvar(TipoEquipe entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            var tipoEquipe = BuscarPorId(entity.Id) ?? entity;
            tipoEquipe.Id = entity.Id;
            tipoEquipe.Descricao = entity.Descricao;
            tipoEquipe.DataInsercao = entity.DataInsercao;
            tipoEquipe.Ativo = entity.Ativo;

            Servico.Salvar(tipoEquipe);
        }

        public bool ObjetoValido(TipoEquipe entity)
        {
            if (string.IsNullOrEmpty(entity.Descricao))
                throw new BusinessRuleException("Informe o Tipo de Equipe!");

            return true;
        }
    }
}