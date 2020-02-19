using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface IItemFuncionarioRepositorio : IRepository<ItemFuncionario>
    {
        void DeleteOrphan();
        void AtualizarEstoque(ItemFuncionario itemFuncionario, ItemFuncionario itemFuncionarioAntigo);
    }
}