using Aplicacao.ViewModels;
using System.Collections.Generic;

namespace Portal.Helpers
{
    public static class TipoAtividade
    {
        public static List<TipoAtividadeViewModel> RetornaTipoAtividades(List<CheckListAtividadeTipoAtividadeViewModel> CheckListAtividadeTipoAtividades)
        {
            var TipoAtividades = new List<TipoAtividadeViewModel>();
            foreach (var CheckListAtividadeTipoAtividade in CheckListAtividadeTipoAtividades)
            {
                TipoAtividades.Add(CheckListAtividadeTipoAtividade.TipoAtividade);
            }

            return TipoAtividades;
        }


        public static List<CheckListAtividadeTipoAtividadeViewModel> RetornaCheckListAtividadeTipoAtividades(int idCheckListAtividade, List<TipoAtividadeViewModel> TipoAtividades)
        {
            var CheckListAtividadeTipoAtividades = new List<CheckListAtividadeTipoAtividadeViewModel>();
            foreach (var TipoAtividade in TipoAtividades)
            {
                var CheckListAtividadeTipoAtividade = new CheckListAtividadeTipoAtividadeViewModel();
                CheckListAtividadeTipoAtividade.CheckListAtividade = idCheckListAtividade;
                CheckListAtividadeTipoAtividade.TipoAtividade = new TipoAtividadeViewModel();
                CheckListAtividadeTipoAtividade.TipoAtividade = TipoAtividade;
                CheckListAtividadeTipoAtividades.Add(CheckListAtividadeTipoAtividade);
            }

            return CheckListAtividadeTipoAtividades;
        }
    }
}