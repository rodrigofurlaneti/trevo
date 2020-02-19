using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;

namespace Portal.Decorators
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        protected virtual CustomPrincipal CurrentUser => HttpContext.Current.User as CustomPrincipal;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (filterContext.HasMarkerAttribute<AllowAnonymousAttribute>())
                    return;

                var loggedUser = CurrentUser;
                if (loggedUser == null)
                    throw new AuthorizationException("Usuário não logado.");

                // Pega as permissões do cache.
                var permissoesDoUsuario = new List<string>();

                foreach (var perfilId in CurrentUser.PerfilsId)
                {
                    if (Core.CacheLayer.Get<IEnumerable<string>>(perfilId.ToString())?.ToList() != null)
                        permissoesDoUsuario.AddRange(Core.CacheLayer.Get<IEnumerable<string>>(perfilId.ToString()).ToList());
                }

                if (!permissoesDoUsuario.Any())
                    throw new AuthorizationException("Não foi possível obter as permissões do usuário.");

                // Ignora validação se o usuário tiver permissão root.
                if (permissoesDoUsuario.Contains("root"))
                    return;

                // Valida se a action está nas regras de permissão do usuário.
                var permissaoRequisitada = $"{filterContext.ActionDescriptor.ControllerDescriptor.ControllerName}_{filterContext.ActionDescriptor.ActionName}".ToLowerInvariant();
                if (!permissoesDoUsuario.Contains(permissaoRequisitada))
                    throw new AuthorizationException("Usuário não possui acesso a esta ação.");

                base.OnAuthorization(filterContext);

                if (filterContext.Result is HttpUnauthorizedResult)
                    RedirectToLogin(filterContext);
            }
            catch (Exception ex)
            {
                filterContext.Controller.TempData["AuthMsg"] = ex.Message;
                RedirectToLogin(filterContext);
            }
        }

        private void RedirectToLogin(AuthorizationContext filterContext)
        {
            filterContext.Controller.TempData["AuthMsg"] = "O usuário não tem acesso à este recurso.";
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Login" }, { "controller", "Account" }, { "returnUrl", filterContext.RequestContext.HttpContext.Request.Url } });
        }
    }
}