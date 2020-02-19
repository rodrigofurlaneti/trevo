using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class ItemFuncionarioViewModel
    {
        public int ItemFuncionarioId { get; set; }
        public DateTime DataInsercao { get; set; }
        public FuncionarioViewModel Funcionario { get; set; }
        public FuncionarioViewModel ResponsavelEntrega { get; set; }
        public DateTime? DataEntrega { get; set; }
        public FuncionarioViewModel ResponsavelDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public List<ItemFuncionarioDetalheViewModel> ItemFuncionariosDetalhes { get; set; }

        public ItemFuncionarioViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}