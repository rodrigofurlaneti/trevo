﻿using System;
using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Event;
using Repositorio.Mapeamento;
using Environment = NHibernate.Cfg.Environment;

namespace Repositorio.Base
{
    /// <summary>
    /// Uses Xml-configuration for setup-config and for mappings is Hbm-files used.
    /// </summary>
    public class FluentNHibContextTest : NHibContext
    {
        public FluentNHibContextTest()
        {

            if (SessionFactory == null)
                SessionFactory = CreateSessionFactory();
        }

        /// <summary>
        /// Creates and returns a session factory.
        /// </summary>
        /// <returns></returns>
        private ISessionFactory CreateSessionFactory()
        {
            const string fileName = @"app.config";
            var map = new ExeConfigurationFileMap { ExeConfigFilename = $"{AppDomain.CurrentDomain.BaseDirectory}/{fileName}" };
            var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            return !string.IsNullOrEmpty(configFile.ConnectionStrings.ConnectionStrings["ConnectionStringMySQL"]?.ConnectionString) ? MySQL() : SQLServer();
        }
        
        private ISessionFactory MySQL()
        {
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionStringMySQL"))
                )
                //.Cache(c => c.UseQueryCache().ProviderClass(typeof(NHibernate.Caches.SysCache2.SysCacheProvider).AssemblyQualifiedName))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PessoaMap>()//x => x.GetProperty("Codigo") != null
                .Conventions.Setup(GetConventions()))
                .ExposeConfiguration(cfg => cfg.SetProperty(Environment.CurrentSessionContextClass, "thread_static"))
                .ExposeConfiguration(cfg => cfg.SetProperty(Environment.UseQueryCache, "true"))
                .ExposeConfiguration(cfg => cfg.SetListener(ListenerType.PostUpdate, new AuditEventListener()))
                //Atualiza o banco com as novas entidades adicionadas
                //.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                //-----------------------------------------------------------------//
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Drop(false, true))
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                //Cuidado essa linha acima cria nova base de dados!
                .CurrentSessionContext("thread_static")
                .BuildSessionFactory();
        }
        
        private ISessionFactory SQLServer()
        {
            return Fluently.Configure() 
                .Database(
                    MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionStringSQL"))
                    //MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionStringSQL"))
                )
                //.Cache(c => c.UseQueryCache().ProviderClass(typeof(NHibernate.Caches.SysCache2.SysCacheProvider).AssemblyQualifiedName))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PessoaMap>()//x => x.GetProperty("Codigo") != null
                .Conventions.Setup(GetConventions()))
                .ExposeConfiguration(cfg => cfg.SetProperty(Environment.CurrentSessionContextClass, "thread_static"))
                .ExposeConfiguration(cfg => cfg.SetListener(ListenerType.PostUpdate, new AuditEventListener()))
                //Atualiza o banco com as novas entidades adicionadas
                //.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                //-----------------------------------------------------------------//
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Drop(false, true))
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                //Cuidado essa linha acima cria nova base de dados!
                .CurrentSessionContext("thread_static")
                .BuildSessionFactory();
        }

        private static Action<IConventionFinder> GetConventions()
        {
            return c =>
                       {
                           c.Add<CascadeConvention>();
                           c.Add<EnumConvention>();
                       };
        }
    }
}