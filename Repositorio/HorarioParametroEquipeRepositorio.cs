using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class HorarioParametroEquipeRepositorio : NHibRepository<HorarioParametroEquipe>, IHorarioParametroEquipeRepositorio
    {
        public HorarioParametroEquipeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}