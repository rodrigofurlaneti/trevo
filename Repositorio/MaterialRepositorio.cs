using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MaterialRepositorio : NHibRepository<Material>, IMaterialRepositorio
    {
        public MaterialRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}