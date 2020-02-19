using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class HorarioPrecoViewModel
    {
        public int? Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Horario { get; set; }
        public decimal Valor { get; set; }
    }
}
