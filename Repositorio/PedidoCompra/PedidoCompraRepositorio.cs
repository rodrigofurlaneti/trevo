using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoCompraRepositorio : NHibRepository<PedidoCompra>, IPedidoCompraRepositorio
    {
        public PedidoCompraRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}