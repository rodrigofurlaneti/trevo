using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class HorarioUnidadeRepositorio : NHibRepository<HorarioUnidade>, IHorarioUnidadeRepositorio
    {
        public HorarioUnidadeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}