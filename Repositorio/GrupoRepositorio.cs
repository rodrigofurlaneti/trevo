using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class GrupoRepositorio : NHibRepository<Grupo>, IGrupoRepositorio
    {
        public GrupoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}