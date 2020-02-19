using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class SelecaoDespesaViewModel
    {
        public int MesVigente { get; set; }
        public int Ano { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public EmpresaViewModel Empresa { get; set; }

        public IList<DespesaContasAPagarViewModel> ContasAPagar { get; set; }
    }
}
