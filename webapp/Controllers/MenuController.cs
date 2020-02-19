using System.Collections.Generic;
using System.Linq;
using Aplicacao;
using Entidade;

namespace Portal.Controllers
{
    public class MenuController : GenericController<Menu>
    {
        public List<Menu> ListaMenus => Aplicacao?.Buscar()?.ToList() ?? new List<Menu>();
        public List<Menu> ListaMenusPai => Aplicacao?.Buscar()?.Where(x => x.MenuPai == null)?.ToList() ?? new List<Menu>();
        public MenuController(IMenuAplicacao menuAplicacao)
        {
            Aplicacao = menuAplicacao;
        }
    }
}