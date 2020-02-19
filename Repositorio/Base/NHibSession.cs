using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Entidade.Base;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Repositorio.Base
{
    public class NHibSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NHibSession"/> class.
        /// </summary>
        public NHibSession()
        {
        }

        private ITransaction Transaction { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }

            if (NHibContext.InnerSession != null)
            {
                NHibContext.InnerSession.Dispose();
                //NHibContext.NHibContext.InnerSession.Close(); Do not use! It breaks when outer transactionscopes are active! Should only use Dispose.
                //NHibContext.InnerSession = null;
            }
        }

        ~NHibSession()
        {
            Dispose(false);
        }

        public void Commit()
        {
            Transaction.Commit();
            SetupNewTransaction();
        }

        public void Rollback()
        {
            Transaction.Rollback();
            SetupNewTransaction();
        }

        public void Clear()
        {
            NHibContext.InnerSession.Clear();
        }

        private void SetupNewTransaction()
        {
            if (Transaction != null)
                Transaction.Dispose();

            Transaction = NHibContext.InnerSession.BeginTransaction();
        }

        /// <summary>
        /// Creates and returns a Query.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        public IQuery CreateQuery(string queryString)
        {
            return NHibContext.InnerSession.CreateQuery(queryString);
        }

        /// <summary>
        /// Creates and returns a Query.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return NHibContext.InnerSession.CreateSQLQuery(queryString);
        }

        /// <summary>
        /// Creates and returns a Query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> func) where T : class, IEntity
        {
            return NHibContext.InnerSession.QueryOver<T>(func);
        }

        /// <summary>
        /// Creates and returns a Criteria.s
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ICriteria CreateCriteria<T>() where T : class, IEntity
        {
            return NHibContext.InnerSession.CreateCriteria<T>();
        }

        /// <summary>
        /// Gets an item that matches sent expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public T GetItemBy<T>(Expression<Func<T, bool>> query) where T : class, IEntity
        {
            return NHibContext.InnerSession.Query<T>().FirstOrDefault(query);
        }

        internal void Flush()
        {
            NHibContext.InnerSession.Flush();
        }

        /// <summary>
        /// Returns item via Id.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TReturn GetItemById<TReturn, TId>(TId id)
        {
            return NHibContext.InnerSession.Get<TReturn>(id);
        }

        /// <summary>
        /// Returns item via NHibernate Criterions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criterions"></param>
        /// <returns></returns>
        public T GetItemByCriterions<T>(params ICriterion[] criterions) where T : class, IEntity
        {
            return AddCriterions(NHibContext.InnerSession.CreateCriteria(typeof(T)), criterions).UniqueResult<T>();
        }

        /// <summary>
        /// Returns a list of ALL items.
        /// </summary>
        /// <remarks>ALL items are returned.</remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> GetList<T>() where T : class, IEntity
        {
            return GetListByCriterions<T>(null);
        }
        
        /// <summary>
        /// Returns a list of items matching sent expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public IList<T> GetListBy<T>(Expression<Func<T, bool>> query) where T : class, IEntity
        {
            return NHibContext.InnerSession.Query<T>().Where(query).ToList();
        }

        /// <summary>
        /// Returns list of item matching sent criterions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criterions"></param>
        /// <returns></returns>
        public IList<T> GetListByCriterions<T>(params ICriterion[] criterions) where T : class, IEntity
        {
            ICriteria criteria = AddCriterions(NHibContext.InnerSession.CreateCriteria(typeof(T)), criterions);
            IList<T> result = criteria.List<T>();

            return result ?? new List<T>(0);
        }

        public IList<T> BuscarPorIntervalo<T>(int registroInicial, int quantidadeRegistros, string colunaOrderBy, params ICriterion[] criterions) where T : class, IEntity
        {
            var createCriteria = NHibContext.InnerSession.CreateCriteria(
                typeof(T)).SetFirstResult(registroInicial).SetMaxResults(quantidadeRegistros);

            if (!string.IsNullOrEmpty(colunaOrderBy))
            {
                createCriteria.AddOrder(Order.Asc(colunaOrderBy));
            }

            ICriteria criteria = AddCriterions(createCriteria, criterions);
            IList<T> result = criteria.List<T>();

            return result ?? new List<T>(0);
        }

        public IList<T> BuscarPorIntervaloOrdernadoPorAlias<T>(int registroInicial, int quantidadeRegistros, string colunaAlias, params ICriterion[] criterions) where T : class, IEntity
        {
            var listaAlias = colunaAlias.Split('.');
            var createCritearia = NHibContext.InnerSession.CreateCriteria(typeof(T))
                                    .SetFirstResult(registroInicial)
                                    .SetMaxResults(quantidadeRegistros);

            for (int i = 0; i < listaAlias.Length - 1; i++)
            {
                var startIndex = i > 0 ? i - 1 : 0;
                var associationPath = string.Join(".", listaAlias, startIndex, i + 1);
                createCritearia.CreateAlias(associationPath, listaAlias[i]);
            }

            var colunaParaOrdenar = string.Join(".", listaAlias, listaAlias.Length - 2, 2);
            createCritearia.AddOrder(Order.Asc(colunaParaOrdenar));

            ICriteria criteria = AddCriterions(createCritearia, criterions);
            IList<T> result = criteria.List<T>();

            return result ?? new List<T>(0);
        }

        /// <summary>
        /// Returns count of the list when entity has as column Codigo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Number of the elements in the select</returns>
        public int GetCountList<T>() where T : class, IEntity
        {
            ICriteria criteria = NHibContext.InnerSession.CreateCriteria(typeof(T))
                                .SetProjection(Projections.CountDistinct("Id"));
            return (int)criteria.UniqueResult();
        }

        /// <summary>
        /// Deletes sent item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Delete<T>(T obj) where T : class, IEntity
        {
            using (ITransaction trans = NHibContext.InnerSession.BeginTransaction())
            {
                NHibContext.InnerSession.Delete(NHibContext.InnerSession.Merge(obj));
                trans.Commit();
            }
        }

        /// <summary>
        /// Deletes sent item by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        public void DeleteById<T, TId>(TId id) where T : class, IEntity
        {
            using (ITransaction trans = NHibContext.InnerSession.BeginTransaction())
            {
                Delete(GetItemById<T, TId>(id));
                trans.Commit();
            }
        }

        /// <summary>
        /// Deletes by query.
        /// </summary>
        /// <param name="query"></param>
        public void DeleteByQuery(string query)
        {
            using (ITransaction trans = NHibContext.InnerSession.BeginTransaction())
            {
                NHibContext.InnerSession.Delete(query);
                trans.Commit();
            }
        }

        /// <summary>
        /// Save or Inserts sent item.
        /// </summary>
        /// <param name="query"></param>
        public void SaveByQuery(string query)
        {
            using (ITransaction trans = NHibContext.InnerSession.BeginTransaction())
            {
                NHibContext.InnerSession.Clear();
                NHibContext.InnerSession.SaveOrUpdate(query);
                trans.Commit();
            }
        }

        /// <summary>
        /// Save or Inserts sent item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Save<T>(T obj) where T : class, IEntity
        {
            using (var trans = NHibContext.InnerSession.BeginTransaction())
            {
                try
                {
                    if (obj.Id == 0)
                    {
                        obj.DataInsercao = DateTime.Now;
                        NHibContext.InnerSession.Clear();
                        obj.Id = (int)NHibContext.InnerSession.Save(NHibContext.InnerSession.Merge(obj));
                    }
                    else
                    {
                        NHibContext.InnerSession.SaveOrUpdate(NHibContext.InnerSession.Merge(obj));
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save or Inserts sent item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public int SaveAndReturn<T>(T obj) where T : class, IEntity
        {
            using (var trans = NHibContext.InnerSession.BeginTransaction())
            {
                NHibContext.InnerSession.Clear();
                var item = NHibContext.InnerSession.Save(NHibContext.InnerSession.Merge(obj));
                trans.Commit();
                return (int) item;
            }
        }

        /// <summary>
        /// Inserts sent item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Insert<T>(T obj) where T : class, IEntity
        {
            using (ITransaction trans = NHibContext.InnerSession.BeginTransaction())
            {
                NHibContext.InnerSession.Clear();
                NHibContext.InnerSession.Save(obj);
                trans.Commit();
            }
        }

        /// <summary>
        /// Updates sent item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Update<T>(T obj) where T : class, IEntity
        {
            using (ITransaction trans = NHibContext.InnerSession.BeginTransaction())
            {
                NHibContext.InnerSession.Clear();
                NHibContext.InnerSession.Update(NHibContext.InnerSession.Merge(obj));
                trans.Commit();
            }
        }

        private static ICriteria AddCriterions(ICriteria criteria, ICriterion[] criterions)
        {
            if (criterions != null)
                criteria = criterions.Aggregate(criteria, (current, t) => current.Add(t));

            return criteria;
        }

        private int count = 0;
        /// <summary>
        /// Save many itens.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Save<T>(IList<T> obj) where T : class, IEntity
        {
            count = 0;
            using (var trans = NHibContext.InnerSession.BeginTransaction())
            {
                try
                {
                    foreach (var t in obj)
                    {
                        if (t.Id == 0)
                        {
                            t.DataInsercao = DateTime.Now;
                            NHibContext.InnerSession.Clear();
                            t.Id = (int)NHibContext.InnerSession.Save(NHibContext.InnerSession.Merge(t));
                            count++;
                            continue;
                        }

                        NHibContext.InnerSession.SaveOrUpdate(NHibContext.InnerSession.Merge(t));
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }

    public class LikeExpressionEscaped : LikeExpression
    {
        private static string EscapeValue(string value)
        {
            var chars = value.ToCharArray();
            var strs = chars.Select(x => x.ToString()).ToArray();
            return "\\" + string.Join("\\", strs);
        }

        public LikeExpressionEscaped(string propertyName, string value, MatchMode matchMode, bool ignoreCase)
            : base(propertyName, EscapeValue(value), matchMode, '\\', ignoreCase)
        { }
    }
}