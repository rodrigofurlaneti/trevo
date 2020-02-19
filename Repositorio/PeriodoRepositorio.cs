using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PeriodoRepositorio : NHibRepository<Periodo>,IPeriodoRepositorio
    {
        public PeriodoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
