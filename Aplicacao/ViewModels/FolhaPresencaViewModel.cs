using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class FolhaPresencaViewModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public string Observacao { get; set; }
        public List<FolhaPresencaFuncionarioViewModel> FolhaPresencaFuncionarios { get; set; }

        public FolhaPresencaViewModel()
        {
        }

        public FolhaPresencaViewModel(int? ano, int? mes, string observacao, List<FolhaPresencaFuncionarioViewModel> folhaPresencaFuncionarios)
        {
            this.Ano = ano ?? DateTime.Now.Year;
            this.Mes = mes ?? DateTime.Now.Month;
            this.Observacao = observacao;
            this.FolhaPresencaFuncionarios = folhaPresencaFuncionarios;
        }
    }
}