using Aplicacao;
using CommonServiceLocator.SimpleInjectorAdapter;
using Dominio;
using Dominio.IRepositorio;
using Microsoft.Practices.ServiceLocation;
using Repositorio;
using Repositorio.Base;
using System.Linq;
using Container = SimpleInjector.Container;

namespace InitializerHelper.Ioc
{
    public class FabricaSimpleInjector
    {
        public static void Registrar(Container container)
        {
            RegistrarNhibernate(container);
            RegistrarAplicacao(container);
            RegistrarServicos(container);
            RegistrarRepositorios(container);

            ServiceLocator.SetLocatorProvider(() => new SimpleInjectorServiceLocatorAdapter(container));
        }

        private static void RegistrarNhibernate(Container container)
        {
            container.Register<NHibContext, FluentNHibContext>();
        }

        private static void RegistrarAplicacao(Container container)
        {
            var repositoryAssembly = typeof(CidadeAplicacao).Assembly;

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "Aplicacao"
                where type.Name.Contains("Aplicacao")
                where type.IsClass
                where type.GetInterfaces().Length > 0
                select new
                {
                    Service = type.GetInterfaces().First(x => x.Name.Equals("I" + type.Name)),
                    Implementation = type
                };

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation);
            }
        }

        private static void RegistrarServicos(Container container)
        {
            var repositoryAssembly = typeof(CidadeServico).Assembly;

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "Dominio" 
                where type.Name.Contains("Servico")
                where type.IsClass
                where type.GetInterfaces().Length > 0
                select new
                {
                    Service = type.GetInterfaces().First(x => x.Name.Equals("I" + type.Name)),
                    Implementation = type
                };

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation);
            }
        }
        
        private static void RegistrarRepositorios(Container container)
        {
            var repositoryAssembly = typeof(CidadeRepositorio).Assembly;

            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Namespace == "Repositorio" 
                where type.Name.Contains("Repositorio")
                where type.IsClass
                where type.GetInterfaces().Length > 0
                select new
                {
                    Service = type.GetInterfaces().First(x => x.Name.Equals("I" + type.Name)),
                    Implementation = type
                };

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation);
            }
        }
    }
}