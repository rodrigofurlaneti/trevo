using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEstruturaGaragemServico : IBaseServico<EstruturaGaragem>
    {
    }

    public class EstruturaGaragemServico : BaseServico<EstruturaGaragem, IEstruturaGaragemRepositorio>, IEstruturaGaragemServico
    {
    }
}