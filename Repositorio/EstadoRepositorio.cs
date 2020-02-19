using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EstadoRepositorio : NHibRepository<Estado>, IEstadoRepositorio
    {
        public EstadoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}