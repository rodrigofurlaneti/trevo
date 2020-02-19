using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IConsolidaFaturamentoAplicacao : IBaseAplicacao<ConsolidaFaturamento>
    {

    }
    public class ConsolidaFaturamentoAplicacao : BaseAplicacao<ConsolidaFaturamento, IConsolidaFaturamentoServico>, IConsolidaFaturamentoAplicacao
    {
    }
}
