using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICargoServico : IBaseServico<Cargo>
    {
    }

    public class CargoServico : BaseServico<Cargo, ICargoRepositorio>, ICargoServico
    {
    }
}