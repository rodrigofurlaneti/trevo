using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TabelaPrecoMensalRepositorio : NHibRepository<TabelaPrecoMensal>, ITabelaPrecoMensalRepositorio
    {
        public TabelaPrecoMensalRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}
