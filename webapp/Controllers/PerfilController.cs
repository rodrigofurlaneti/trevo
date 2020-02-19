using System.Collections.Generic;
using System.Linq;
using Entidade;
using Aplicacao;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class PerfilController : GenericController<Perfil>
    {

        public List<Perfil> ListaPerfils => Aplicacao?.Buscar()?.ToList() ?? new List<Perfil>();
        public List<Menu> ListaMenus
        {
            get
            {
                if (Session["ListaMenusPerfil"] == null)
                    Session["ListaMenusPerfil"] = _perfilAplicacao.BuscaMenus()?.ToList() ?? new List<Menu>();
                return (List<Menu>)Session["ListaMenusPerfil"];
            }
            set { Session["ListaMenusPerfil"] = value; }
        }

        private readonly IPerfilAplicacao _perfilAplicacao;

        public PerfilController(IPerfilAplicacao perfilAplicacao)
        {
            Aplicacao = perfilAplicacao;
            _perfilAplicacao = perfilAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            Session.Remove("ListaMenusPerfil");
            ListaMenus = _perfilAplicacao.BuscaMenus()?.ToList() ?? new List<Menu>();
            return base.Index();
        }

    }
}