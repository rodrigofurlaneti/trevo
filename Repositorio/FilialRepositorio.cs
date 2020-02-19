using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class FilialRepositorio : NHibRepository<Filial>, IFilialRepositorio
    {
        public FilialRepositorio(NHibContext context) : base(context)
        {
        }
    }
}
