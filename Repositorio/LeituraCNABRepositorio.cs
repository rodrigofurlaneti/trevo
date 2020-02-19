using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class LeituraCNABRepositorio : NHibRepository<LeituraCNAB>, ILeituraCNABRepositorio
    {
        public LeituraCNABRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}