using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IParametroFaturamentoAplicacao : IBaseAplicacao<ParametroFaturamento>
    {
    }

    public class ParametroFaturamentoAplicacao : BaseAplicacao<ParametroFaturamento, IParametroFaturamentoServico>, IParametroFaturamentoAplicacao
    {
    }
}