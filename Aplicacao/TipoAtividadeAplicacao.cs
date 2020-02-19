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
    public interface ITipoAtividadeAplicacao : IBaseAplicacao<TipoAtividade>
    { }

    public class TipoAtividadeAplicacao : BaseAplicacao<TipoAtividade, ITipoAtividadeServico>, ITipoAtividadeAplicacao
    {
        private readonly ICidadeAplicacao _cidadeAplicacao;

        public new void Salvar(TipoAtividade entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            var tipoAtividade = BuscarPorId(entity.Id) ?? entity;
            tipoAtividade.Id = entity.Id;
            tipoAtividade.Descricao = entity.Descricao;
            tipoAtividade.DataInsercao = entity.DataInsercao;
            tipoAtividade.Ativo = entity.Ativo;

            Servico.Salvar(tipoAtividade);
        }

        public bool ObjetoValido(TipoAtividade entity)
        {
            if (string.IsNullOrEmpty(entity.Descricao))
                throw new BusinessRuleException("Informe o Tipo de Atividade!");

            return true;
        }
    }
}