using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class CheckListAtividadeTipoAtividadeViewModel
    {
        public TipoAtividadeViewModel TipoAtividade { get; set; }
        public int CheckListAtividade { get; set; }

        public CheckListAtividadeTipoAtividadeViewModel()
        {
            TipoAtividade = new TipoAtividadeViewModel();
        }

        public CheckListAtividadeTipoAtividadeViewModel(CheckListAtividadeTipoAtividade CheckListAtividadeTipoAtividade)
        {
            TipoAtividade = new TipoAtividadeViewModel(CheckListAtividadeTipoAtividade?.TipoAtividade ?? new TipoAtividade());
            CheckListAtividade = CheckListAtividadeTipoAtividade.CheckListAtividade;
        }

        public CheckListAtividadeTipoAtividade ToEntity()
        {
            return new CheckListAtividadeTipoAtividade
            {
                TipoAtividade = TipoAtividade?.ToEntity(),
                CheckListAtividade = CheckListAtividade
            };
        }

        public CheckListAtividadeTipoAtividade ToEntity(CheckListAtividadeTipoAtividadeViewModel CheckListAtividadeTipoAtividade)
        {
            return new CheckListAtividadeTipoAtividade
            {
                DataInsercao = DateTime.Now,
                TipoAtividade = CheckListAtividadeTipoAtividade.TipoAtividade?.ToEntity(),
                CheckListAtividade = CheckListAtividadeTipoAtividade.CheckListAtividade
            };
        }

        public CheckListAtividadeTipoAtividadeViewModel ToViewModel(CheckListAtividadeTipoAtividade CheckListAtividadeTipoAtividade)
        {
            return new CheckListAtividadeTipoAtividadeViewModel
            {
                TipoAtividade = new TipoAtividadeViewModel(CheckListAtividadeTipoAtividade?.TipoAtividade ?? new TipoAtividade()),
                CheckListAtividade = CheckListAtividadeTipoAtividade.CheckListAtividade
            };
        }

        public IList<CheckListAtividadeTipoAtividadeViewModel> ListaTipoAtividades(IList<CheckListAtividadeTipoAtividade> CheckListAtividadeTipoAtividades)
        {
            var CheckListAtividadeTipoAtividadesViewModel = new List<CheckListAtividadeTipoAtividadeViewModel>();
            foreach (var TipoAtividade in CheckListAtividadeTipoAtividades)
            {
                CheckListAtividadeTipoAtividadesViewModel.Add(ToViewModel(TipoAtividade));
            }

            return CheckListAtividadeTipoAtividadesViewModel;
        }

        public IList<CheckListAtividadeTipoAtividade> ListaTipoAtividades(IList<CheckListAtividadeTipoAtividadeViewModel> CheckListAtividadeTipoAtividades)
        {
            var CheckListAtividadeTipoAtividadesViewModel = new List<CheckListAtividadeTipoAtividade>();
            foreach (var TipoAtividade in CheckListAtividadeTipoAtividades)
            {
                CheckListAtividadeTipoAtividadesViewModel.Add(ToEntity(TipoAtividade));
            }

            return CheckListAtividadeTipoAtividadesViewModel;
        }
    }
}
