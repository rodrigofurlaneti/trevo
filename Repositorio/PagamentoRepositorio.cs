using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PagamentoRepositorio : NHibRepository<Pagamento>, IPagamentoRepositorio
    {
        public PagamentoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}