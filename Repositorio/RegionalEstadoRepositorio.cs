using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class RegionalEstadoRepositorio : NHibRepository<RegionalEstado>, IRegionalEstadoRepositorio
    {
        public RegionalEstadoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}