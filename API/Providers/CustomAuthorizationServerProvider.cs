using Dominio;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace API.Providers
{
    public class CustomAuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        private readonly Container _container;
        public CustomAuthorizationServerProvider(Container container)
        {
            _container = container;
        }
        public override Task AuthorizationEndpointResponse(OAuthAuthorizationEndpointResponseContext context)
        {
            return base.AuthorizationEndpointResponse(context);
        }
        public override Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            return base.AuthorizeEndpoint(context);
        }
        public override Task GrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context)
        {
            return base.GrantAuthorizationCode(context);
        }
        public override Task ValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
        {
            return base.ValidateAuthorizeRequest(context);
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var clientId = string.Empty;
            var clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "Empty userLogin");
                return Task.FromResult<object>(null);
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                context.SetError("invalid_clientSecret", "Empty clientkey");
                return Task.FromResult<object>(null);
            }

            using (SimpleInjector.Lifestyles.AsyncScopedLifestyle.BeginScope(_container))
            {
                var userService = _container.GetInstance<IUsuarioServico>();
                var user = userService.ValidarLogin(clientId, clientSecret);

                if (user == null)
                {
                    context.SetError("invalid_Authentication", "Invalid authentication");
                    return Task.FromResult<object>(null);
                }
            }
            context.Validated(clientId);
            //return Task.FromResult<object>(null);
            return base.ValidateClientAuthentication(context);
        }
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            // create identity
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.ClientId));

            //Adiciona os dados do cliente autenticado, neste caso, o id da organização da tabela dealernet_integrations

            string login;
            using (SimpleInjector.Lifestyles.AsyncScopedLifestyle.BeginScope(_container))
            {
                var userAppService = _container.GetInstance<IUsuarioServico>();
                var objLogin = userAppService.RetornarPorCPF(context.ClientId);
                login = objLogin.Login;
                identity.AddClaim(new Claim("userLogin", login));
                identity.AddClaim(new Claim("userId", objLogin.Id.ToString()));
            }

            // create metadata to pass on to refresh token provider
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { "as:client_id", context.ClientId }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

            ////context.Validated(identity);
            //return Task.FromResult<object>(null);
            return base.GrantClientCredentials(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}