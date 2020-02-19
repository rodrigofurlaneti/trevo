using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ITipoNotificacaoAplicacao : IBaseAplicacao<TipoNotificacao>
    {
    }

    public class TipoNotificacaoAplicacao : BaseAplicacao<TipoNotificacao, ITipoNotificacaoServico>, ITipoNotificacaoAplicacao
    {
    }
}