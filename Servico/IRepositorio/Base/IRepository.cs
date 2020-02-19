using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entidade.Base;

namespace Dominio.IRepositorio.Base
{
    public interface IRepository<T> where T : class, IEntity
    {
        void Commit();
        void Rollback();
        void Clear();

        void Save(T item);
        int SaveAndReturn(T item);
        void Insert(T item);
        void Update(T item);
        void Delete(T item);
        void DeleteById(int id);
        void Dispose();

        T FirstBy(Expression<Func<T, bool>> query);
        T GetById(int id);

        IList<T> List();
        IList<T> ListBy(Expression<Func<T, bool>> query);
        IList<T> BuscarPorIntervalo(int registroInicial, int quantidadeRegistros, string colunaOrderBy = "");
        IList<T> BuscarPorIntervaloOrdernadoPorAlias(int registroInicial, int quantidadeRegistros, string colunaAlias);

        int Count();
        void Save(IList<T> item);
        void Flush();
    }
}
