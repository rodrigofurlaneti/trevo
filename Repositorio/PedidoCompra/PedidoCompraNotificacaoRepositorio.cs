using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoCompraNotificacaoRepositorio : NHibRepository<PedidoCompraNotificacao>, IPedidoCompraNotificacaoRepositorio
    {
        public PedidoCompraNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}