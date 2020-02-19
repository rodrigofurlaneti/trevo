using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IFaturamentoServico : IBaseServico<Faturamento>
    {

    }

    public class FaturamentoServico : BaseServico<Faturamento, IFaturamentoRepositorio>, IFaturamentoServico
    {
    }
}
