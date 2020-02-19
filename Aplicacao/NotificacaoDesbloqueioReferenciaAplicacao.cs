using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao
{
    public interface INotificacaoDesbloqueioReferenciaAplicacao : IBaseAplicacao<NotificacaoDesbloqueioReferencia>
    {
        void Criar(int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, int idUsuario, string msgErro = null, string urlPersonalizada = null);
        void AtualizarStatus(int idNotificacao, int idUsuarioAcao, AcaoNotificacao status);
        void ConsumirLiberacao(int idNotificacao, bool utilizado = false);
    }

    public class NotificacaoDesbloqueioReferenciaAplicacao : BaseAplicacao<NotificacaoDesbloqueioReferencia, INotificacaoDesbloqueioReferenciaServico>, INotificacaoDesbloqueioReferenciaAplicacao
    {
        private readonly INotificacaoDesbloqueioReferenciaServico _notificacaoDesbloqueioReferenciaServico;

        public NotificacaoDesbloqueioReferenciaAplicacao(INotificacaoDesbloqueioReferenciaServico notificacaoDesbloqueioReferenciaServico)
        {
            _notificacaoDesbloqueioReferenciaServico = notificacaoDesbloqueioReferenciaServico;
        }

        public void AtualizarStatus(int idNotificacao, int idUsuarioAcao, AcaoNotificacao status)
        {
            _notificacaoDesbloqueioReferenciaServico.AtualizarStatus(idNotificacao, idUsuarioAcao, status);
        }

        public void ConsumirLiberacao(int idNotificacao, bool utilizado = false)
        {
            _notificacaoDesbloqueioReferenciaServico.ConsumirLiberacao(idNotificacao, utilizado);
        }

        public void Criar(int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, int idUsuario, string msgErro = null, string urlPersonalizada = null)
        {
            _notificacaoDesbloqueioReferenciaServico.Criar(idRegistro, entidadeRegistro, dataReferencia, idUsuario, msgErro, urlPersonalizada);
        }
    }
}
