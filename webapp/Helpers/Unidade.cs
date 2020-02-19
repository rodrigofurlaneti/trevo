using Aplicacao.ViewModels;
using Entidade;
using System.Collections.Generic;

namespace Portal.Helpers
{
    public static class Unidade
    {

        public static IList<EstruturaUnidadeViewModel> ListaUnidadeEstruturaUnidade(IList<EstruturaGaragemViewModel> estruturasGaragem)
        {
            var unidadeEstruturasUnidade = new List<EstruturaUnidadeViewModel>();
            foreach(var estruturaGaragem in estruturasGaragem)
            {
                var estruturaUnidade = new EstruturaUnidadeViewModel();
                estruturaUnidade.EstruturaGaragem = new EstruturaGaragemViewModel();
                estruturaUnidade.EstruturaGaragem = estruturaGaragem;
                unidadeEstruturasUnidade.Add(estruturaUnidade);
            }

            return unidadeEstruturasUnidade;
        }

        public static IList<CheckListEstruturaUnidade> ListaCheckListEquipamentoUnidade(IList<EstruturaGaragemViewModel> estruturasGaragem)
        {
            var unidadeEstruturasUnidade = new List<CheckListEstruturaUnidade>();
            foreach (var estruturaGaragem in estruturasGaragem)
            {
                var estruturaUnidade = new CheckListEstruturaUnidade();
                
                estruturaUnidade.EstruturaGaragem = estruturaGaragem.Descricao;
                unidadeEstruturasUnidade.Add(estruturaUnidade);
            }

            return unidadeEstruturasUnidade;
        }
    }
}