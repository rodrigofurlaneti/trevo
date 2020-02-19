using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Aplicacao.ViewModels;
using Entidade.Base;

namespace Aplicacao.Base
{
    public interface IBaseAplicacao<T> where T : IEntity
    {
        T BuscarPorId(int id);
        IList<T> Buscar();
        void Salvar(T entity);
        T SalvarComRetorno(T entity);
        void Excluir(T entity);
        void ExcluirPorId(int id);
        T PrimeiroPor(Expression<Func<T, bool>> query);
        IList<T> BuscarPor(Expression<Func<T, bool>> query);
        IList<T> BuscarPorIntervalo(int registroInicial, int quantidadeRegistros, string colunaOrderBy = "");
        IList<T> BuscarPorIntervaloOrdernadoPorAlias(int registroInicial, int quantidadeRegistros, string colunaAlias);
        int Contar();
        void Salvar(IList<T> entity);
        IList<Audit> BuscarLogPor(Expression<Func<Audit, bool>> query);

        IEnumerable<ChaveValorViewModel> BuscarValoresDoEnum<T>();
    }
}
