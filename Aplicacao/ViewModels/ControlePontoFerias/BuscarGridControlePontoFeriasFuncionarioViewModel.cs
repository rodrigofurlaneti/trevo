using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class BuscarGridControlePontoFeriasFuncionarioViewModel
    {
        public int? UnidadeId { get; set; }
        public int? SupervisorId { get; set; }
        public int? FuncionarioId { get; set; }
        public string ColunaUnidade { get; set; }
        public string ColunaSupervisor { get; set; }
        public string ColunaFuncionario { get; set; }
    }
}