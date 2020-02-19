using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoLocacaoNotificacaoRepositorio : NHibRepository<PedidoLocacaoNotificacao>, IPedidoLocacaoNotificacaoRepositorio
    {
        public PedidoLocacaoNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
