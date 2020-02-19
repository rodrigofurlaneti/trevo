using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IParametroNotificacaoUsuarioAplicacao: IBaseAplicacao<ParametroNotificacaoUsuario>
    {

    }

    public class ParametroNotificacaoUsuarioAplicacao : BaseAplicacao<ParametroNotificacaoUsuario, IParametroNotificacaoUsuarioServico>, IParametroNotificacaoUsuarioAplicacao
    {
    }
}
