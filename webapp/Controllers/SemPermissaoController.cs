using Entidade;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class SemPermissaoController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}