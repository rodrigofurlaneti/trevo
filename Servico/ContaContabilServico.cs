using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface IContaContabilServico : IBaseServico<ContaContabil>
    {
        IList<ContaContabil> BuscarDadosSimples();
    }
    
    public class ContaContabilServico : BaseServico<ContaContabil, IContaContabilRepositorio>, IContaContabilServico
    {
        private readonly IContaContabilRepositorio _contaContabilRepositorio;

        public ContaContabilServico(IContaContabilRepositorio contaContabilRepositorio)
        {
            _contaContabilRepositorio = contaContabilRepositorio;
        }

        public IList<ContaContabil> BuscarDadosSimples()
        {
            return _contaContabilRepositorio.BuscarDadosSimples();
        }
    }
}