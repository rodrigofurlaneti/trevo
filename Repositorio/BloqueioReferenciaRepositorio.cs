using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class BloqueioReferenciaRepositorio : NHibRepository<BloqueioReferencia>, IBloqueioReferenciaRepositorio
    {
        public BloqueioReferenciaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}