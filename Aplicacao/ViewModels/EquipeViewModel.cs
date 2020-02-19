using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class EquipeViewModel
    {
        public int Id { get; set; }
        public  DateTime DataInsercao { get; set; }
        public  string Nome { get; set; }
        public  DateTime Datafim { get; set; }
        public  bool Ativo { get; set; }
        public  UnidadeViewModel Unidade { get; set; }
        public TipoEquipeViewModel TipoEquipe { get; set; }
        //public FuncionamentoViewModel HorarioTrabalho { get; set; }
        //public HorarioUnidadeViewModel HorarioUnidade { get; set; }
        public TipoHorario TipoHorario { get; set; }
        public string IdEncarregado { get; set; }
        public FuncionarioViewModel Encarregado { get; set; }
        public string IdSupervisor { get; set; }
        public FuncionarioViewModel Supervisor { get; set; }
        public IList<ColaboradorViewModel> Colaboradores { get; set; }
        //public IList<EquipeColaboradorViewModel> Colaboradores { get; set; }

        public EquipeViewModel(Equipe equipe)
        {
            Id = equipe.Id;
            Nome = equipe.Nome;
        }
        public EquipeViewModel()
        {

        }
    }
}
