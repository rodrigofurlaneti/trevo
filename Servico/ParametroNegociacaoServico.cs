using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametroNegociacaoServico : IBaseServico<ParametroNegociacao>
    {

    }

    public class ParametroNegociacaoServico : BaseServico<ParametroNegociacao, IParametroNegociacaoRepositorio>, IParametroNegociacaoServico
    {
    }
}
