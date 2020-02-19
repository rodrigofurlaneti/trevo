using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class EquipeColaboradorViewModel
    {
        public ColaboradorViewModel Colaborador { get; set; }
        public EquipeColaboradorViewModel()
        {
            Colaborador = new ColaboradorViewModel();
        }
    }
}
