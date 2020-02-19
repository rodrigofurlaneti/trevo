using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MenuRepositorio : NHibRepository<Menu>, IMenuRepositorio
    {
        public MenuRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}