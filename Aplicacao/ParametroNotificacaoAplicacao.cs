using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IParametroNotificacaoAplicacao: IBaseAplicacao<ParametroNotificacao>
    {

    }

    public class ParametroNotificacaoAplicacao : BaseAplicacao<ParametroNotificacao, IParametroNotificacaoServico>, IParametroNotificacaoAplicacao
    {
    }
}
