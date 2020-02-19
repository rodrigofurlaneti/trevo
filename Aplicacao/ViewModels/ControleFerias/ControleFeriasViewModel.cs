using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class ControleFeriasViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public bool AutorizadoTrabalhar { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public List<ControleFeriasPeriodoPermitidoViewModel> ListaPeriodoPermitido { get; set; }
        public FuncionarioViewModel Funcionario { get; set; }

        public ControleFeriasViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}