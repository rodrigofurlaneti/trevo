using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IConsolidaFaturamentoServico : IBaseServico<ConsolidaFaturamento>
    {

    }
    public class ConsolidaFaturamentoServico : BaseServico<ConsolidaFaturamento, IConsolidaFaturamentoRepositorio>, IConsolidaFaturamentoServico
    {
        
    }
}
