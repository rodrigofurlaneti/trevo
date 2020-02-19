using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ConvenioUnidadeRepositorio : NHibRepository<ConvenioUnidade>, IConvenioUnidadeRepositorio
    {
        public ConvenioUnidadeRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}
