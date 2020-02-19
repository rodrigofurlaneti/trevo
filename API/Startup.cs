using Microsoft.Owin;
using Owin;
using SimpleInjector;
using System.Reflection;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;
using SimpleInjector.Lifestyles;
using Microsoft.Owin.Security.OAuth;
using InitializerHelper.Ioc;
using Microsoft.Owin.Cors;
using System;
using System.Configuration;
using API.Providers;


[assembly: OwinStartup(typeof(API.Startup))]
namespace API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            var config = new HttpConfiguration();
           
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration, Assembly.GetExecutingAssembly());

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            ConfigureAuth(app, container);

            // ativando configuração WebApi
            app.UseWebApi(config);
        }

        private static void InitializeContainer(Container container)
        {
            FabricaSimpleInjector.Registrar(container);
        }

        public void ConfigureAuth(IAppBuilder app, Container container)
        {
            var OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Authorize"),
                ApplicationCanDisplayErrors = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["TokenExpireTimeSpan"].ToString())),
                Provider = new CustomAuthorizationServerProvider(container),
                AuthorizationCodeProvider = new CustomAuthorizationCodeProvider(),
                RefreshTokenProvider = new CustomRefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}

