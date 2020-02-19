using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IFaturamentoAplicacao : IBaseAplicacao<Faturamento>
    {
    }
    public class FaturamentoAplicacao : BaseAplicacao<Faturamento, IFaturamentoServico>, IFaturamentoAplicacao
    {
    }
}
