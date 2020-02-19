using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class PermissaoViewModel
    {
        public virtual string Nome { get; set; }
        public virtual string Regra { get; set; }
        public virtual IList<PerfilViewModel> Perfis { get; set; }

        public PermissaoViewModel()
        {
            Perfis = new List<PerfilViewModel>();
        }
    }
}