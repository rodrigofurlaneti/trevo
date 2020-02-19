using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PeriodoHorarioRepositorio : NHibRepository<PeriodoHorario>, IPeriodoHorarioRepositorio
    {
        public PeriodoHorarioRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}