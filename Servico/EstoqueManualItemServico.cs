using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEstoqueManualItemServico : IBaseServico<EstoqueManualItem>
    {
    }

    public class EstoqueManualItemServico : BaseServico<EstoqueManualItem, IEstoqueManualItemRepositorio>, IEstoqueManualItemServico
    {
    }
}