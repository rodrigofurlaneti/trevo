using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoPagamentoRepositorio : NHibRepository<TipoPagamento>, ITipoPagamentoRepositorio
    {
        public TipoPagamentoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
