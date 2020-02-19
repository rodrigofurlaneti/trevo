using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PermissaoRepositorio : NHibRepository<Permissao>, IPermissaoRepositorio
    {
        public PermissaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}