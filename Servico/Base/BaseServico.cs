using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominio.IRepositorio.Base;
using Entidade.Base;
using Microsoft.Practices.ServiceLocation;

namespace Dominio.Base
{
    public class BaseServico<TEntity, TRepositorio> : IBaseServico<TEntity> where TEntity : class, IEntity where TRepositorio : IRepository<TEntity>
    {
        private IRepository<TEntity> _repositorio;
        public IRepository<TEntity> Repositorio => _repositorio ?? (_repositorio = GetRepositorio());

        protected virtual TRepositorio GetRepositorio()
        {
            return ServiceLocator.Current.GetInstance<TRepositorio>();
        }

        public IList<TEntity> Buscar()
        {
            return Repositorio.List();
        }

        public TEntity BuscarPorId(int id)
        {
            return Repositorio.GetById(id);
        }

        public virtual void Salvar(TEntity entity)
        {
            Repositorio.Save(entity);
        }

        public int SalvarComRetorno(TEntity entity)
        {
            return Repositorio.SaveAndReturn(entity);
        }

        public void Excluir(TEntity entity)
        {
            Repositorio.Delete(entity);
        }

        public virtual void ExcluirPorId(int id)
        {
            Repositorio.DeleteById(id);
        }

        public int Contar()
        {
            return Repositorio.Count();
        }

        public void Commit()
        {
            Repositorio.Commit();
        }

        public void Clear()
        {
            Repositorio.Clear();
        }

        public void Salvar(IList<TEntity> itens)
        {
            Repositorio.Save(itens);
        }

        public TEntity PrimeiroPor(Expression<Func<TEntity, bool>> query)
        {
            return Repositorio.FirstBy(query);
        }

        public IList<TEntity> BuscarPor(Expression<Func<TEntity, bool>> query)
        {
            return Repositorio.ListBy(query);
        }

        public IList<TEntity> BuscarPorIntervalo(int registroInicial, int quantidadeRegistros, string colunaOrderBy)
        {
            return Repositorio.BuscarPorIntervalo(registroInicial, quantidadeRegistros, colunaOrderBy);
        }

        public IList<TEntity> BuscarPorIntervaloOrdernadoPorAlias(int registroInicial, int quantidadeRegistros, string colunaAlias)
        {
            return Repositorio.BuscarPorIntervaloOrdernadoPorAlias(registroInicial, quantidadeRegistros, colunaAlias);
        }
    }
}