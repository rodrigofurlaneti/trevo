using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EquipeRepositorio : NHibRepository<Equipe>, IEquipeRepositorio
    {
        public EquipeRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
