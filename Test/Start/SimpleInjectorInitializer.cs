using System.Web.Mvc;
using Test.InitializerHelper.Ioc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace Test.Start
{
    public static class SimpleInjectorInitializer
    {
        public static Container Container { get; set; }

        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            InitializeContainer(container);
            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
        
        private static void InitializeContainer(Container container)
        {
            FabricaSimpleInjector.Registrar(container);
            Container = container;
        }
    }
}