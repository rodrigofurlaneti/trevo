using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametroNumeroNotaFiscalServico : IBaseServico<ParametroNumeroNotaFiscal>
    {

    }

    public class ParametroNumeroNotaFiscalServico : BaseServico<ParametroNumeroNotaFiscal, IParametroNumeroNotaFiscalRepositorio>, IParametroNumeroNotaFiscalServico
    {

    }
}
