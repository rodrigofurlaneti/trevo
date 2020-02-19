using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoEquipeRepositorio : NHibRepository<TipoEquipe>, ITipoEquipeRepositorio
    {
        public TipoEquipeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}