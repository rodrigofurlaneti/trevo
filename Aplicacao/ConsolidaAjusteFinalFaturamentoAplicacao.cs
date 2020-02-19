using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IConsolidaAjusteFinalFaturamentoAplicacao : IBaseAplicacao<ConsolidaAjusteFinalFaturamento>
    {

    }
    public class ConsolidaAjusteFinalFaturamentoAplicacao : BaseAplicacao<ConsolidaAjusteFinalFaturamento, IConsolidaAjusteFinalFaturamentoServico>, IConsolidaAjusteFinalFaturamentoAplicacao
    {

    }
}
