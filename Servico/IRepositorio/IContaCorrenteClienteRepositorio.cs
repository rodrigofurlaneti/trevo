using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface IContaCorrenteClienteRepositorio : IRepository<ContaCorrenteCliente>
    {
        void DeleteOrphan();
    }
}