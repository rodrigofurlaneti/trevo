using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class ItemFuncionarioImpressaoViewModel
    {
        public FuncionarioViewModel Funcionario { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public List<ItemFuncionarioDetalheViewModel> ItensSelecionados { get; set; }

        public ItemFuncionarioImpressaoViewModel()
        {
        }

        public ItemFuncionarioImpressaoViewModel(FuncionarioViewModel funcionario, DateTime? dataEntrega, DateTime? dataDevolucao, List<ItemFuncionarioDetalheViewModel> itensSelecionados)
        {
            Funcionario = funcionario;
            DataEntrega = dataEntrega;
            DataDevolucao = dataDevolucao;
            ItensSelecionados = itensSelecionados;
        }
    }
}