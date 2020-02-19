using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class UnidadeCheckListAtividadeViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public int Unidade { get; set; }
        public bool Checkado { get; set; }
        public CheckListAtividadeViewModel CheckListAtividade { get; set; }
        public virtual StatusCheckList StatusCheckList { get; set; }

        public UnidadeCheckListAtividadeViewModel()
        {

        }

        public UnidadeCheckListAtividadeViewModel(UnidadeCheckListAtividade unidadeCheckListAtividade)
        {
            Id = unidadeCheckListAtividade.Id;
            DataInsercao = unidadeCheckListAtividade.DataInsercao;
            Unidade = unidadeCheckListAtividade.Unidade;
            StatusCheckList = unidadeCheckListAtividade.StatusCheckList;
            CheckListAtividade = new CheckListAtividadeViewModel(unidadeCheckListAtividade?.CheckListAtividade ?? new CheckListAtividade());
        }

        public UnidadeCheckListAtividade ToEntity()
        {
            return new UnidadeCheckListAtividade
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Unidade = Unidade,
                StatusCheckList =StatusCheckList,
                CheckListAtividade = CheckListAtividade?.ToEntity()
            };
        }

        public UnidadeCheckListAtividadeViewModel ToEntity(UnidadeCheckListAtividade unidadeCheckListAtividade)
        {
            return new UnidadeCheckListAtividadeViewModel
            {
                Id = unidadeCheckListAtividade.Id,
                DataInsercao = unidadeCheckListAtividade.DataInsercao,
                Unidade = unidadeCheckListAtividade.Unidade,
                StatusCheckList = unidadeCheckListAtividade.StatusCheckList,
                CheckListAtividade = new CheckListAtividadeViewModel(unidadeCheckListAtividade?.CheckListAtividade ?? new CheckListAtividade())
            };
        }
    }
}
