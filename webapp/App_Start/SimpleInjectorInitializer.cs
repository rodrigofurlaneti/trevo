using System.Reflection;
using System.Web.Mvc;
using InitializerHelper.Ioc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace Portal
{
    public static class SimpleInjectorInitializer
    {
        public static Container Container;
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            InitializeContainer(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
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