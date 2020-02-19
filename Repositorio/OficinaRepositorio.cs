using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class OficinaRepositorio : NHibRepository<Oficina>, IOficinaRepositorio
    {
        public OficinaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}