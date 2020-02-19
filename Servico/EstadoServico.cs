using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEstadoServico : IBaseServico<Estado>
    {
    }

    public class EstadoServico : BaseServico<Estado, IEstadoRepositorio>, IEstadoServico
    {
    }
}