using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class BeneficioFuncionarioViewModel
    {
        public int BeneficioFuncionarioId { get; set; }
        public DateTime DataInsercao { get; set; }
        public FuncionarioViewModel Funcionario { get; set; }
        public List<BeneficioFuncionarioDetalheViewModel> BeneficioFuncionarioDetalhes { get; set; }

        public BeneficioFuncionarioViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}
