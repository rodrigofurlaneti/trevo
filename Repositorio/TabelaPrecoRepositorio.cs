using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TabelaPrecoRepositorio : NHibRepository<TabelaPreco>, ITabelaPrecoRepositorio
    {
        public TabelaPrecoRepositorio(NHibContext context): base(context)
        {

        }
    }
}
