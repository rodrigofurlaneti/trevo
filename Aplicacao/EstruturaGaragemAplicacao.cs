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
    public interface IEstruturaGaragemAplicacao : IBaseAplicacao<EstruturaGaragem>
    {
        List<EstruturaGaragemViewModel> BuscarAtivos();
    }

    public class EstruturaGaragemAplicacao : BaseAplicacao<EstruturaGaragem, IEstruturaGaragemServico>, IEstruturaGaragemAplicacao
    {
        private readonly ICidadeAplicacao _cidadeAplicacao;

        public new void Salvar(EstruturaGaragem entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            var EstruturaGaragem = BuscarPorId(entity.Id) ?? entity;
            EstruturaGaragem.Id = entity.Id;
            EstruturaGaragem.Descricao = entity.Descricao;
            EstruturaGaragem.DataInsercao = entity.DataInsercao;
            EstruturaGaragem.Ativo = entity.Ativo;

            Servico.Salvar(EstruturaGaragem);
        }

        public bool ObjetoValido(EstruturaGaragem entity)
        {
            if (string.IsNullOrEmpty(entity.Descricao))
                throw new BusinessRuleException("Informe o Tipo de Estrutura de Garagem!");

            return true;
        }

        public List<EstruturaGaragemViewModel> BuscarAtivos()
        {
            return Servico.BuscarPor(x => x.Ativo)?.Select(x => new EstruturaGaragemViewModel(x))?.ToList() ?? new List<EstruturaGaragemViewModel>();
        }
    }
}