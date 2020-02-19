using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IMenuServico : IBaseServico<Menu>
    {
        bool VerificaMenuJaAssociado(int idMenu);
        bool ExisteMenuDuplicado(Menu menu);
    }

    public class MenuServico : BaseServico<Menu, IMenuRepositorio>, IMenuServico
    {
        private readonly IPerfilServico _perfilServico;

        public MenuServico(IPerfilServico perfilServico)
        {
            _perfilServico = perfilServico;
        }
        public bool VerificaMenuJaAssociado(int idMenu)
        {
            var perfis = _perfilServico.BuscarPor(x => x.Menus.Any(y => y.Menu.Id == idMenu));
            return perfis != null && perfis.Any();
        }

        public bool ExisteMenuDuplicado(Menu menu)
        {
            var menus = BuscarPor(x => x.Url != null && x.Url.Length > 0 
            && x.Url.Equals(!menu.Url.Substring(0, 1).Equals("/") ? $"/{menu.Url}" : menu.Url) || x.Url.Equals(menu.Url.Substring(0, 1).Equals("/") ? menu.Url.Substring(1) : menu.Url)
            && x.MenuPai.Id == menu.MenuPai.Id);

            return menus != null && menus.Any(x=>x.Id != menu.Id);
        }
    }
}