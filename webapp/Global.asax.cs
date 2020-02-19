using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using Aplicacao.Mappers;
using Aplicacao.ViewModels;
using InitializerHelper.Startup;
using log4net;

namespace Portal
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SimpleInjectorInitializer.Initialize();
            AutoMapperConfig.RegisterMappings();
            AppStartup.Initialize();

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;
            
            var serializer = new JavaScriptSerializer();
            var authticketOld = FormsAuthentication.Decrypt(authCookie.Value);
            var serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authticketOld?.UserData ?? string.Empty);
            var userData = serializer.Serialize(serializeModel);
            var authTicket = new FormsAuthenticationTicket(1, serializeModel.Login, authticketOld?.IssueDate ?? DateTime.Now, DateTime.Now.AddMinutes(30), serializeModel.RememberMe, userData);
            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Clear();
            Response.Cookies.Add(faCookie);
            
            var newUser = new CustomPrincipal(authTicket.Name)
            {
                UsuarioId = serializeModel.UsuarioId,
                PessoaId = serializeModel.PessoaId,
                Nome = serializeModel.Nome,
                PerfilsId = serializeModel.PerfilsId,
                RememberMe = serializeModel.RememberMe,
                //Imagem = (byte[])HttpContext.Current.Session?["UserAvatar"]
            };

            HttpContext.Current.User = newUser;
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            log.Error(ex.TargetSite?.DeclaringType?.Name ?? "App_Error", ex);
        }
    }
}