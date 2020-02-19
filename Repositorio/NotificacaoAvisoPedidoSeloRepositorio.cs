using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class NotificacaoAvisoPedidoSeloRepositorio : NHibRepository<NotificacaoAvisoPedidoSelo>, INotificacaoAvisoPedidoSeloRepositorio
    {
        public NotificacaoAvisoPedidoSeloRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}