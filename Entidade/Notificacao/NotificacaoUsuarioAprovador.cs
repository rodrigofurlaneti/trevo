namespace Entidade
{
    public class NotificacaoUsuarioAprovador
    {
        public virtual Usuario UsuarioAprovador { get; set; }
        public virtual Notificacao Notificacao { get; set; }
    }
}