using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IContaContabilRepositorio : IRepository<ContaContabil>
    {
        IList<ContaContabil> BuscarDadosSimples();
    }
}