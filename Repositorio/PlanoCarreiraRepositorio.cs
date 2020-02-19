using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PlanoCarreiraRepositorio : NHibRepository<PlanoCarreira>, IPlanoCarreiraRepositorio
    {
        public PlanoCarreiraRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}