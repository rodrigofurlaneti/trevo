using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class FolhaPresencaFuncionarioViewModel
    {
        public FuncionarioViewModel Funcionario { get; set; }
        public List<FolhaPresencaDiaViewModel> FolhaPresencaDias { get; set; }
        public bool Selecionado { get; set; }

        public FolhaPresencaFuncionarioViewModel(Funcionario funcionario)
        {
            this.Funcionario = new FuncionarioViewModel(funcionario);
        }
    }
}