using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface IContaFinanceiraRepositorio : IRepository<ContaFinanceira>
    {
        ContaFinanceira BuscarContaPadrao();
    }
}