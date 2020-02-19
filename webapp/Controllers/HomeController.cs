using System.Web.Mvc;
using Portal.Decorators;


namespace Portal.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        // GET: home/index
        [CheckSessionOut]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}