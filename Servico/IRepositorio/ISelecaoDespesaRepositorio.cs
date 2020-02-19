using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface ISelecaoDespesaRepositorio : IRepository<SelecaoDespesa>
    {
        void RemoverPorContaAPagarId(int id);
    }
}
