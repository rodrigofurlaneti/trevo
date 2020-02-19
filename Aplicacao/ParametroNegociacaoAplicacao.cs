using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IParametroNegociacaoAplicacao: IBaseAplicacao<ParametroNegociacao>
    {

    }

    public class ParametroNegociacaoAplicacao : BaseAplicacao<ParametroNegociacao, IParametroNegociacaoServico>, IParametroNegociacaoAplicacao
    {
    }
}
