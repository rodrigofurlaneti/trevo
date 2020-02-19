using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class AluguelRepositorio : NHibRepository<Aluguel>, IAluguelRepositorio
    {
        public AluguelRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
