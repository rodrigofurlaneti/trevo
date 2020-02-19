using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class RemanejamentoTransferenciaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public TipoEquipeViewModel TipoEquipe { get; set; }
        public EquipeViewModel Equipe { get; set; }
        public HorarioPrecoViewModel Horario { get; set; }
    }
}
