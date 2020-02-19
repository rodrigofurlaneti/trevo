using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PagamentoMensalidadeRepositorio : NHibRepository<PagamentoMensalidade>, IPagamentoMensalidadeRepositorio
    {
        public PagamentoMensalidadeRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
