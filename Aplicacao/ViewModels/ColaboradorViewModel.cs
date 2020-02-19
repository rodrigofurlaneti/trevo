using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class ColaboradorViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public PeriodoHorarioViewModel Turno { get; set; }
        public string IdNomeColaborador { get; set; }
        public FuncionarioViewModel NomeColaborador { get; set; }
        public string IdTurno { get; set; }
        public string IdColaborador { get; set; }
    }
}
