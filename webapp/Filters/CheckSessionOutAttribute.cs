using Aplicacao;
using Aplicacao.ViewModels;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckSessionOutAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var _perfilAplicacao = ServiceLocator.Current.GetInstance<IPerfilAplicacao>();

            var context = HttpContext.Current;
            var user = context.User as CustomPrincipal;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).Any())
                    return;

                context.Session.Abandon();
                FormsAuthentication.SignOut();
                context.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
                {
                    Expires = DateTime.Now.AddYears(-1)
                };
                context.Response.Cookies.Clear();
                context.Response.Cookies.Add(cookie);
                if (!string.IsNullOrEmpty(context.Request.RawUrl))
                {
                    string redirectTo = $"~/Account/Login?ReturnUrl={HttpUtility.UrlEncode(context.Request.RawUrl)}";
                    filterContext.Result = new RedirectResult(redirectTo);

                }
                else
                {
                    string redirectTo = $"~/Account/Login";
                    filterContext.Result = new RedirectResult(redirectTo);
                }
                return;
            }

            var teste = Environment.GetEnvironmentVariables();

#if !DEBUG
            if(!filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(PermitirQualquerPerfilAttribute), false).Any() &&
                !filterContext.ActionDescriptor.GetCustomAttributes(typeof(PermitirQualquerPerfilAttribute), false).Any() &&
                !_perfilAplicacao.VerificarSeTemAcessoAoMenu(user.PerfilsId, context.Request.Path))
                filterContext.Result = new RedirectResult("~/SemPermissao/Index");
#endif

            base.OnActionExecuting(filterContext);
        }
    }
}