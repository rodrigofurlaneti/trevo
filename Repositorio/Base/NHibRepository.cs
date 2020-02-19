using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using Core.Exceptions;
using Dominio.IRepositorio.Base;
using Entidade.Base;

namespace Repositorio.Base
{
    public abstract class NHibRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        protected NHibRepository(NHibContext context)
        {
            Context = context;
            Session = Context.CreateNewSession();
        }

        protected NHibContext Context { get; }
        protected NHibSession Session { get; }

        public bool IsMySql()
        {
            const string fileName = @"Web.Config";
            var map = new ExeConfigurationFileMap { ExeConfigFilename = $"{AppDomain.CurrentDomain.BaseDirectory}/{fileName}" };
            var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            return !string.IsNullOrEmpty(configFile.ConnectionStrings.ConnectionStrings["ConnectionStringMySQL"]?.ConnectionString);
        }

        public bool IsSql()
        {
            const string fileName = @"Web.Config";
            var map = new ExeConfigurationFileMap { ExeConfigFilename = $"{AppDomain.CurrentDomain.BaseDirectory}/{fileName}" };
            var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            return !string.IsNullOrEmpty(configFile.ConnectionStrings.ConnectionStrings["ConnectionStringSQL"]?.ConnectionString);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Session.Dispose();
        }

        #endregion

        #region IRepository<T> Members

        public void Commit()
        {
            Session.Commit();
        }

        public void Rollback()
        {
            Session.Rollback();
        }

        public void Clear()
        {
            Session.Clear();
        }

        public virtual void Save(T item)
        {
            Session.Save(item);
        }

        public virtual int SaveAndReturn(T item)
        {
            return Session.SaveAndReturn(item);
        }

        public virtual void Insert(T item)
        {
            Session.Insert(item);
        }

        public virtual void Update(T item)
        {
            Session.Update(item);
        }

        public virtual void Delete(T item)
        {
            Session.Delete(item);
        }

        public virtual T FirstBy(Expression<Func<T, bool>> query)
        {
            return Session.GetItemBy(query);
        }

        public T GetById(int id)
        {
            return FirstBy(x => x.Id.Equals(id));
        }

        public virtual IList<T> List()
        {
            return Session.GetList<T>();
        }

        public virtual IList<T> BuscarPorIntervalo(int registroInicial, int quantidadeRegistros, string colunaOrderBy = "")
        {
            return Session.BuscarPorIntervalo<T>(registroInicial, quantidadeRegistros, colunaOrderBy, null);
        }

        public IList<T> BuscarPorIntervaloOrdernadoPorAlias(int registroInicial, int quantidadeRegistros, string colunaAlias)
        {
            return Session.BuscarPorIntervaloOrdernadoPorAlias<T>(registroInicial, quantidadeRegistros, colunaAlias, null);
        }

        public virtual IList<T> ListBy(Expression<Func<T, bool>> query)
        {
            return Session.GetListBy(query);
        }

        public virtual int Count()
        {
            return Session.GetCountList<T>();
        }

        public int GetCountList()
        {
            return Session.GetCountList<T>();
        }

        public void DeleteById(int id)
        {
            var entity = GetById(id);
            if (entity == null)
                throw new NotFoundException($"Este item não existe ou já foi excluido");

            Delete(entity);
        }

        public virtual void Save(IList<T> itens)
        {
            Session.Save(itens);
        }

        public void Flush()
        {
            Session.Flush();
        }
        
        #endregion
    }
}