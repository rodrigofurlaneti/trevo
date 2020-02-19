using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class OISRepositorio : NHibRepository<OIS>, IOISRepositorio
    {
        public OISRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}