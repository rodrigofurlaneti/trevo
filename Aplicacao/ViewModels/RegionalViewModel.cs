using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class RegionalViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public IList<RegionalEstadoViewModel> Estados { get; set; }
    }
}
