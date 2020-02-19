using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class HorarioPrecoRepositorio : NHibRepository<HorarioPreco>, IHorarioPrecoRepositorio
    {
        public HorarioPrecoRepositorio(NHibContext context)
          : base(context)
        {
        }
    }
}
