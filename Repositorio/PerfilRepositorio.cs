using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PerfilRepositorio : NHibRepository<Perfil>, IPerfilRepositorio
    {
        public PerfilRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}