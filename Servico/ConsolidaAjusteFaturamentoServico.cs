using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IConsolidaAjusteFaturamentoServico : IBaseServico<ConsolidaAjusteFaturamento>
    {
      

    }

    public class ConsolidaAjusteFaturamentoServico : BaseServico<ConsolidaAjusteFaturamento, IConsolidaAjusteFaturamentoRepositorio>, IConsolidaAjusteFaturamentoServico
    {
       

    }
}
