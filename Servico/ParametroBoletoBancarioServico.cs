using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametroBoletoBancarioServico : IBaseServico<ParametroBoletoBancario>
    {
    }

    public class ParametroBoletoBancarioServico : BaseServico<ParametroBoletoBancario, IParametroBoletoBancarioRepositorio>, IParametroBoletoBancarioServico
    {
    }
}