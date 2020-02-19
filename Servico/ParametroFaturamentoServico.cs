using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametroFaturamentoServico : IBaseServico<ParametroFaturamento>
    {
    }

    public class ParametroFaturamentoServico : BaseServico<ParametroFaturamento, IParametroFaturamentoRepositorio>, IParametroFaturamentoServico
    {
    }
}