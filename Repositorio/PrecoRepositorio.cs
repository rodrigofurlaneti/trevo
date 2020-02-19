using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PrecoRepositorio : NHibRepository<Preco>, IPrecoRepositorio
    {
        public PrecoRepositorio(NHibContext context)
          : base(context)
        {
        }
    }
}
