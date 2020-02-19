using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;
using Entidade.Uteis;
using AutoMapper;

namespace Aplicacao
{
    public interface ICheckListAtividadeAplicacao : IBaseAplicacao<CheckListAtividade>
    {
        List<CheckListAtividadeViewModel> BuscarAtivos();
    }

    public class CheckListAtividadeAplicacao : BaseAplicacao<CheckListAtividade, ICheckListAtividadeServico>, ICheckListAtividadeAplicacao
    {
        public new void Salvar(CheckListAtividade entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            var CheckListAtividade = BuscarPorId(entity.Id) ?? entity;
            CheckListAtividade.Id = entity.Id;
            CheckListAtividade.Descricao = entity.Descricao;
            CheckListAtividade.DataInsercao = entity.DataInsercao;
            CheckListAtividade.Ativo = entity.Ativo;
            CheckListAtividade.TiposAtividade = entity.TiposAtividade;

            Servico.Salvar(CheckListAtividade);
        }

        public bool ObjetoValido(CheckListAtividade entity)
        {
            if (string.IsNullOrEmpty(entity.Descricao))
                throw new BusinessRuleException("Informe o Tipo de Atividade!");

            if (entity.Responsavel == null || entity.Responsavel.Pessoa == null)
                throw new BusinessRuleException("Informe um Responsável!");

            return true;
        }

        public List<CheckListAtividadeViewModel> BuscarAtivos()
        {
            return Mapper.Map<List<CheckListAtividadeViewModel>>(Servico.BuscarPor(x => x.Ativo));
        }
    }
}