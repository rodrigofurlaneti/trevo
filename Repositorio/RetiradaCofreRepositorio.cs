using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class RetiradaCofreRepositorio : NHibRepository<RetiradaCofre>, IRetiradaCofreRepositorio
    {
        public RetiradaCofreRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}