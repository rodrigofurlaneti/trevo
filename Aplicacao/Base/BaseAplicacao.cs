using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominio.Base;
using Entidade.Base;
using Microsoft.Practices.ServiceLocation;
using Dominio;
using Aplicacao.ViewModels;
using System.Linq;
using Core.Extensions;
using System.Collections;

namespace Aplicacao.Base
{
    public class BaseAplicacao<TEntity, TServico> : IBaseAplicacao<TEntity> where TEntity : class, IEntity where TServico : IBaseServico<TEntity>
    {
        private IBaseServico<TEntity> _servico;
        public IBaseServico<TEntity> Servico => _servico ?? (_servico = GetServico());

        protected virtual TServico GetServico()
        {
            return ServiceLocator.Current.GetInstance<TServico>();
        }

        public IList<TEntity> Buscar()
        {
            return Servico.Buscar();
        }

        public TEntity BuscarPorId(int id)
        {
            return Servico.BuscarPorId(id);
        }

        public void Salvar(TEntity entity)
        {
            Servico.Salvar(entity);
        }

        public void Excluir(TEntity entity)
        {
            Servico.Excluir(entity);
        }

        public virtual void ExcluirPorId(int id)
        {
            Servico.ExcluirPorId(id);
        }

        public int Contar()
        {
            return Servico.Contar();
        }

        public TEntity PrimeiroPor(Expression<Func<TEntity, bool>> query)
        {
            return Servico.PrimeiroPor(query);
        }

        public IList<TEntity> BuscarPor(Expression<Func<TEntity, bool>> query)
        {
            return Servico.BuscarPor(query);
        }

        public IList<TEntity> BuscarPorIntervalo(int registroInicial, int quantidadeRegistros, string colunaOrderBy = "")
        {
            return Servico.BuscarPorIntervalo(registroInicial, quantidadeRegistros, colunaOrderBy);
        }

        public IList<TEntity> BuscarPorIntervaloOrdernadoPorAlias(int registroInicial, int quantidadeRegistros, string colunaAlias)
        {
            return Servico.BuscarPorIntervaloOrdernadoPorAlias(registroInicial, quantidadeRegistros, colunaAlias);
        }

        public TEntity SalvarComRetorno(TEntity entity)
        {
            var id = Servico.SalvarComRetorno(entity);

            return BuscarPorId(id);
        }

        public void Salvar(IList<TEntity> entity)
        {
            Servico.Salvar(entity);
        }
        
        public IList<Audit> BuscarLogPor(Expression<Func<Audit, bool>> query)
        {
            return new AuditServico().BuscarPor(query);
        }

        public IEnumerable<ChaveValorViewModel> BuscarValoresDoEnum<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(e => new ChaveValorViewModel { Id = Convert.ToInt32(e), Descricao = e.ToDescription() });
        }
    }
}