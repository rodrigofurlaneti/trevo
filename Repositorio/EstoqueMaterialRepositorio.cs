using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EstoqueMaterialRepositorio : NHibRepository<EstoqueMaterial>, IEstoqueMaterialRepositorio
    {
        public EstoqueMaterialRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}