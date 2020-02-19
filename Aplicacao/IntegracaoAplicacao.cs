using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IIntegracaoAplicacao : IBaseAplicacao<Integracao>
    {
    }

    public class IntegracaoAplicacao : BaseAplicacao<Integracao, IIntegracaoServico>, IIntegracaoAplicacao
    {
    }
}