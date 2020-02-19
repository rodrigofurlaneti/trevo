using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TabelaPrecoAvulsaRepositorio : NHibRepository<TabelaPrecoAvulsa>, ITabelaPrecoAvulsaRepositorio
    {
        public TabelaPrecoAvulsaRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}
