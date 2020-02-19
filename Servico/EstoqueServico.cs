using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEstoqueServico : IBaseServico<Estoque>
    {
    }

    public class EstoqueServico : BaseServico<Estoque, IEstoqueRepositorio>, IEstoqueServico
    {
    }
}