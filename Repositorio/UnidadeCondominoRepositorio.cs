using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class UnidadeCondominoRepositorio : NHibRepository<UnidadeCondomino>, IUnidadeCondominoRepositorio
    {
        public UnidadeCondominoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}