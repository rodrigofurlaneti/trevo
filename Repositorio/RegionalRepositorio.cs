using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class RegionalRepositorio : NHibRepository<Regional>, IRegionalRepositorio
    {
        public RegionalRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}