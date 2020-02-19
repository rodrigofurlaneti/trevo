using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TabelaPrecoMensalistaUnidadeRepositorio : NHibRepository<TabelaPrecoMensalistaUnidade>, ITabelaPrecoMensalistaUnidadeRepositorio
    {
        public TabelaPrecoMensalistaUnidadeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}