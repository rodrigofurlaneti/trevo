using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoMaterialRepositorio : NHibRepository<TipoMaterial>, ITipoMaterialRepositorio
    {
        public TipoMaterialRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}