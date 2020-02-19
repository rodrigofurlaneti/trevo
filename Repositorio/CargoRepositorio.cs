using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
namespace Repositorio
{
    public class CargoRepositorio : NHibRepository<Cargo>, ICargoRepositorio
    {
        public CargoRepositorio(NHibContext context)
            :base(context)
        {

        }
    }
}
