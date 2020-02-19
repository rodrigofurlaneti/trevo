using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CalendarioRHRepositorio : NHibRepository<CalendarioRH>, ICalendarioRHRepositorio
    {
        public CalendarioRHRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}