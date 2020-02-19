using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class OcorrenciaFuncionarioViewModel
    {
        public int OcorrenciaFuncionarioId { get; set; }
        public DateTime DataInsercao { get; set; }
        public FuncionarioViewModel Funcionario { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public UsuarioViewModel UsuarioResponsavel { get; set; }
        public List<OcorrenciaFuncionarioDetalheViewModel> OcorrenciaFuncionarioDetalhes { get; set; }
        public string ValorTotal { get; set; }

        public OcorrenciaFuncionarioViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}