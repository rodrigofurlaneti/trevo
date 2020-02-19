using System.Collections.Generic;
using System.Web.Mvc;
using Aplicacao.ViewModels;
using System.Linq;

namespace Portal.Controllers
{
    public class BaseController : Controller
    {
        public List<MenuViewModel> MenusUsuario
        {
            get { return (List<MenuViewModel>)Session["MenusUsuario"] ?? new List<MenuViewModel>(); }
            set { Session["MenusUsuario"] = value; }
        }

        public string ActionName => ControllerContext.RouteData.Values["action"].ToString();

        public string ControllerName => ControllerContext.RouteData.Values["controller"].ToString();

        public DadosModal DadosModal { get; set; }

        public DadosValidacaoNotificacaoDesbloqueioReferenciaModal DadosValidacaoDesbloqueioReferenciaModal { get; set; }

        public DadosModal GerarDadosModal(string titulo, string msg, TipoModal tipo,
            string acaoConfirma = null, string tituloConfirma = null, int? id = null, string redirectUrl = null, string acaoFunction = null)
        {
            DadosModal = new DadosModal
            {
                Titulo = titulo,
                Mensagem = msg,
                TipoModal = tipo
            };

            if (acaoFunction != null)
                DadosModal.AcaoFunction = acaoFunction;

            if (acaoConfirma != null)
                DadosModal.AcaoConfirma = acaoConfirma;
                
            if (tituloConfirma != null)
                DadosModal.TituloConfirma = tituloConfirma;
                
            if (id != null)
                DadosModal.Id = id.Value;
                
            if (redirectUrl != null)
                DadosModal.RedirectUrl = redirectUrl;

			if (tipo == TipoModal.Success && string.IsNullOrEmpty(DadosModal.RedirectUrl))
				DadosModal.RedirectUrl = $"/{ControllerName}/Index";

            return DadosModal;
        }
        
        public MenuViewModel BuscarMenu()
        {
            return MenusUsuario.FirstOrDefault(x => x.Url != null
            && ((x.Url.ToLower().Substring(0, 1).Contains("/") ? x.Url : $"/{x.Url}").ToLower().Contains($"/{ControllerName.ToLower()}/{ActionName}")
                || (x.Url.ToLower().Substring(0, 1).Contains("/") ? x.Url : $"/{x.Url}").ToLower().Contains($"/{ControllerName.ToLower()}/index")));
        }
    }
}