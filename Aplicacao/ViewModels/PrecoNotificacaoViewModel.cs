
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class PrecoNotificacaoViewModel
    {
        public NotificacaoViewModel Notificacao { get; set; }
        public PrecoViewModel Preco { get; set; }

        public StatusPrevioNotificacao StatusARealizar { get; set; }
    }
}
