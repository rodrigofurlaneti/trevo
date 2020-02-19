using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class BuscarGridControlePontoFuncionarioViewModel
    {
        public int? UnidadeId { get; set; }
        public int? SupervisorId { get; set; }
        public int? FuncionarioId { get; set; }
        public string ColunaUnidade { get; set; }
        public string ColunaSupervisor { get; set; }
        public string ColunaFuncionario { get; set; }
        public int? Ano { get; set; }
        public int? Mes { get; set; }
    }
}