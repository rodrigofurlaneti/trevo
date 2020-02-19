using System.Collections.Generic;
using System.Web.Mvc;
using Aplicacao.ViewModels;
using Entidade;
using Newtonsoft.Json;

namespace Portal.Controllers
{
    public class ContatoController : GenericController<Contato>
    {
        private List<ContatoViewModel> _contatos { get { return (List<ContatoViewModel>)Session["contatos"] ?? new List<ContatoViewModel>(); } }
        
        public ActionResult AtualizarContatos(List<ContatoViewModel> contatos)
        {
            Session["contatos"] = contatos;

            return PartialView("~/Views/Contact/_GridContacts.cshtml", contatos);
        }

        public JsonResult BuscarContatos()
        {
            return Json(_contatos, JsonRequestBehavior.AllowGet);
        }
    }
}