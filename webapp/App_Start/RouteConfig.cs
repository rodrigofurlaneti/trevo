using System.Web.Mvc;
using System.Web.Routing;
using Portal.App_Helpers;

namespace Portal
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;

            routes.MapRoute("AlterarStatus", "{controller}/{action}/{id}/{status}", new
            {
                controller = "ArquivoImportacao",
                action = "AlterarStatus"
            }).RouteHandler = new DashRouteHandler();

            //routes.MapRoute("Contrato", "{controller}/{action}/{id}/{idCarteira}", new
            //{
            //    controller = "Contrato",
            //    action = "Deletar"
            //}).RouteHandler = new DashRouteHandler();

            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                controller = "Account",
                action = "Login",
                id = UrlParameter.Optional
            }).RouteHandler = new DashRouteHandler();
        }
    }
}