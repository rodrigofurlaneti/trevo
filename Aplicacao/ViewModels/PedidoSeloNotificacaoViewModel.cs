using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class PedidoSeloNotificacaoViewModel 
    {
        public NotificacaoViewModel Notificacao { get; set; }
        public PedidoSeloViewModel PedidoSelo { get; set; }
        public StatusPrevioNotificacao StatusARealizar { get; set; }

        public PedidoSeloNotificacaoViewModel()
        {

        }
    }
}