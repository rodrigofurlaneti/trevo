using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class NotificacaoDesbloqueioReferenciaRepositorio : NHibRepository<NotificacaoDesbloqueioReferencia>, INotificacaoDesbloqueioReferenciaRepositorio
    {
        public NotificacaoDesbloqueioReferenciaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}