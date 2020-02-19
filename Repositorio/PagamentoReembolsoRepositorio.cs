using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PagamentoReembolsoRepositorio : NHibRepository<PagamentoReembolso>, IPagamentoReembolsoRepositorio
    {
        public PagamentoReembolsoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}